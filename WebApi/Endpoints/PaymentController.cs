using Application.Users.Commands;
using Application.Users.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using WebApi.DataTransferObjects.Payments;
using WebApi.Payment;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("payments")]
    [Tags("Payments")]
    [Authorize]
    public class PaymentController(IMediator mediator, SessionService sessionService, PaymentConfiguration paymentConfiguration)
        : ControllerBase
    {
        [HttpPost("checkout", Name = "Create Checkout Session")]
        [EndpointSummary("Create Checkout Session")]
        public async Task<ActionResult<CreateCheckoutSessionResponse>> CreateCheckoutSession(
            [FromBody] CreateCheckoutSessionRequest request,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var cartResult = await mediator.Send(new GetUserCartItemsQuery(currentUser.IdentityId), cancellationToken);
            if (!cartResult.IsSuccess)
            {
                return BadRequest();
            }

            var cartItems = cartResult.Value;
            if (cartItems.Count == 0)
            {
                return BadRequest("The cart is empty.");
            }

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
                ClientReferenceId = currentUser.IdentityId.ToString(),
                Metadata = new Dictionary<string, string>
                {
                    ["UserId"] = currentUser.IdentityId.ToString(),
                    ["GameIds"] = string.Join(',', cartItems.Select(item => item.Game.Id))
                },
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = Convert.ToInt64(decimal.Round(item.Game.Price * (1 - item.Game.Discount / 100) * 100m, 0, MidpointRounding.AwayFromZero)),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Game.Title
                        }
                    }
                }).ToList()
            };

            var session = await sessionService.CreateAsync(options, cancellationToken: cancellationToken);
            return Ok(new CreateCheckoutSessionResponse(session.Id, session.Url ?? throw new InvalidOperationException("Stripe did not return a checkout URL.")));
        }

        [AllowAnonymous]
        [HttpPost("webhook", Name = "Stripe Webhook")]
        [EndpointSummary("Stripe Webhook")]
        public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
            var stripeSignature = Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, paymentConfiguration.WebhookSecret);
            }
            catch (StripeException)
            {
                return BadRequest();
            }

            if (stripeEvent.Type != "checkout.session.completed" || stripeEvent.Data.Object is not Session session)
            {
                return Ok();
            }

            if (session.PaymentStatus != "paid")
            {
                return Ok();
            }

            var userIdValue = session.Metadata.TryGetValue("UserId", out var metadataUserId)
                ? metadataUserId
                : session.ClientReferenceId;

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return BadRequest();
            }

            if (!session.Metadata.TryGetValue("GameIds", out var gameIdsValue))
            {
                return BadRequest();
            }

            foreach (var gameIdValue in gameIdsValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!Guid.TryParse(gameIdValue, out var gameId))
                {
                    return BadRequest();
                }

                await mediator.Send(new AddGameToUserLibraryCommand(userId, gameId), cancellationToken);
            }

            return Ok();
        }
    }
}
