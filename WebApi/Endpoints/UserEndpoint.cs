using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Users.Requests;
using WebApi.DataTransferObjects.Users.Responses;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("users")]
    [Tags("Users")]
    public class UserEndpoint(IMediator mediator, UserMapper mapper)
        : Controller
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Users")]
        [EndpointSummary("Get Users")]
        public async Task<PaginatedResponse<UserListItemResponse>> Get( [FromQuery] GetUsersRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUsersQuery(query.Username, query.PageNumber, query.PageSize), cancellationToken);
            return await mapper.MapToUserPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{id}", Name = "Get User By Id")]
        [EndpointSummary("Get User By Id")]
        public async Task<Result<UserResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);
            return await mapper.MapToUserResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet("me/collections", Name = "Get User Collections")]
        [EndpointSummary("Get User Collections")]
        [Authorize]
        public async Task<PaginatedResponse<GameCollectionListItemResponse>> GetCollections(
            [FromQuery] GetUserCollectionsRequest query,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetUserCollectionsQuery(currentUser.IdentityId, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGameCollectionPaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("me/collections/{collectionId}", Name = "Get User Collection By Id")]
        [EndpointSummary("Get User Collection By Id")]
        [Authorize]
        public async Task<Result<GameCollectionDetailsResponse>> GetCollectionById(
            Guid collectionId,
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetUserCollectionByIdQuery(currentUser.IdentityId, collectionId), cancellationToken);
            return queryResult.Map(mapper.MapToGameCollectionDetailsResponse);
        }

        [TranslateResultToActionResult]
        [HttpPatch("me/profile-picture", Name = "Update Profile Picture")]
        [EndpointSummary("Update Profile Picture")]
        [Authorize]
        public async Task<Result> UpdateProfilePicture( [FromForm] IFormFile formFile, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var fileData = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(currentUser.IdentityId, fileData), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("me", Name = "Update User")]
        [EndpointSummary("Update User")]
        [Authorize]
        public async Task<Result<UpdateUserResponse>> Update( [FromBody] UpdateUserRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(currentUser.IdentityId, request.DisplayName), cancellationToken);
            return commandResult.Map(mapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("me/collections", Name = "Add Game Collection")]
        [EndpointSummary("Add Game Collection")]
        [Authorize]
        public async Task<Result<GameCollectionResponse>> AddGameCollection( [FromBody] CreateGameCollectionRequest createGameCollectionRequest,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CreateUserGameCollectionCommand(currentUser.IdentityId, createGameCollectionRequest.Name), cancellationToken);
            return commandResult.Map(mapper.MapToGameCollectionResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("me/collections/{id}", Name = "Remove Game Collection")]
        [EndpointSummary("Remove Game Collection")]
        [Authorize]
        public async Task<Result> RemoveGameCollection(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveUserGameCollectionCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("me/promote-to-devoloper", Name = "Promote To Developer")]
        [EndpointSummary("Promote To Developer")]
        [Authorize]
        public async Task<Result<UpdateUserResponse>> PromoteToDeveloper(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PromoteUserToDeveloperCommand(currentUser.IdentityId), cancellationToken);
            return commandResult.Map(mapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("me/cart/{id}", Name = "Add Item To Cart")]
        [EndpointSummary("Add Item To Cart")]
        [Authorize]
        public async Task<Result> AddItemToCart(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new AddGameToUserCartCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpDelete("me/cart/{id}", Name = "Remove Item From Cart")]
        [EndpointSummary("Remove Item From Cart")]
        [Authorize]
        public async Task<Result> RemoveItemFromCart(Guid id, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameFromUserCartCommand(currentUser.IdentityId, id), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpGet("me/cart", Name = "Get Cart")]
        [EndpointSummary("Get Cart")]
        [Authorize]
        public async Task<Result<GetUserCartResponse>> GetCart(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetUserCartItemsQuery(currentUser.IdentityId), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGetUserCartResponse(r, cancellationToken));
        }
    }
}
