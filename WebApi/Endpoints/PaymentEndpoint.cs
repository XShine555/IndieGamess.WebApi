using Application.Abstractions.Payment;
using Application.Users.Commands;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObjects.Payments.Requests;
using WebApi.DataTransferObjects.Payments.Responses;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("payments")]
    [Tags("Payments")]
    public class PaymentEndpoint(IMediator mediator, IStripeService stripeService) : Controller
    {
        [TranslateResultToActionResult]
        [HttpPost("checkout", Name = "Create Checkout Session")]
        [EndpointSummary("Create a Stripe Checkout Session for the current user's cart")]
        [Authorize]
        public async Task<Result<CreateCheckoutSessionResponse>> CreateCheckoutSession(
            [FromBody] CreateCheckoutSessionRequest request,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var result = await mediator.Send(
                new CreateStripeCheckoutSessionCommand(currentUser.IdentityId, request.SuccessUrl, request.CancelUrl),
                cancellationToken);

            return result.Map(url => new CreateCheckoutSessionResponse(url));
        }

        [HttpPost("webhook", Name = "Stripe Webhook")]
        [EndpointSummary("Stripe webhook handler - called by Stripe after payment")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook(CancellationToken cancellationToken)
        {
            var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);
            var signature = HttpContext.Request.Headers["Stripe-Signature"].FirstOrDefault() ?? string.Empty;

            var webhookResult = stripeService.ParseCheckoutCompletedEvent(payload, signature);
            if (webhookResult is null)
                return BadRequest();

            var fulfillResult = await mediator.Send(
                new FulfillCartOrderCommand(webhookResult.UserId, webhookResult.GameIds),
                cancellationToken);

            return fulfillResult.IsSuccess ? Ok() : StatusCode(500);
        }
    }
}
