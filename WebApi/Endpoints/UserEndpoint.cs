using Application.Games.Catalog.Queries;
using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Users.Requests;
using WebApi.DataTransferObjects.Users.Responses;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("users")]
    [Tags("Users")]
    public class UserEndpoint(IMediator mediator, IUserMapper userMapper, IGameMapper gameMapper)
        : Controller
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Users")]
        [EndpointSummary("Get Users")]
        public async Task<PaginatedResponse<UserListItemResponse>> Get([FromQuery] GetUsersRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUsersQuery(query.Username, query.PageNumber, query.PageSize), cancellationToken);
            return await userMapper.MapToUserPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{id}", Name = "Get User By Id")]
        [EndpointSummary("Get User By Id")]
        public async Task<Result<UserResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);
            return await userMapper.MapToUserResponse(queryResult, cancellationToken);
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
            return userMapper.MapToGameCollectionPaginatedResponse(queryResult);
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
            return await queryResult.MapAsync(r => userMapper.MapToGameCollectionDetailsResponse(r, cancellationToken));
        }

        [TranslateResultToActionResult]
        [HttpPatch("me/profile-picture", Name = "Update Profile Picture")]
        [EndpointSummary("Update Profile Picture")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<Result> UpdateProfilePicture([FromForm] UpdateUserProfilePictureRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var fileData = FileData.FromFormFile(request.ProfilePicture);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(currentUser.IdentityId, fileData), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpGet("me", Name = "Get Current User")]
        [EndpointSummary("Get Current User")]
        [Authorize]
        public async Task<Result<UserResponse>> GetCurrentUser(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(currentUser.IdentityId), cancellationToken);
            return await userMapper.MapToUserResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpPut("me", Name = "Update User")]
        [EndpointSummary("Update User")]
        [Authorize]
        public async Task<Result<UpdateUserResponse>> Update([FromForm] UpdateUserRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(currentUser.IdentityId, request.DisplayName), cancellationToken);
            return commandResult.Map(userMapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("me/collections", Name = "Add Game Collection")]
        [EndpointSummary("Add Game Collection")]
        [Authorize]
        public async Task<Result<GameCollectionResponse>> AddGameCollection([FromBody] CreateGameCollectionRequest createGameCollectionRequest,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CreateUserGameCollectionCommand(currentUser.IdentityId, createGameCollectionRequest.Name), cancellationToken);
            return commandResult.Map(userMapper.MapToGameCollectionResponse);
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
            return commandResult.Map(userMapper.MapToUpdateUserResponse);
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
            return await queryResult.MapAsync(r => userMapper.MapToGetUserCartResponse(r, cancellationToken));
        }

        [HttpGet("me/created-games", Name = "Get Created Games")]
        [EndpointSummary("Get Created Games")]
        [Authorize]
        public async Task<PaginatedResponse<GameResponse>> GetCreatedGames([FromQuery] GetCreatedGamesRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetCreatedGamesByIdentityIdQuery(
                currentUser.IdentityId,
                request.Title,
                request.PageNumber,
                request.PageSize), cancellationToken);
            return await gameMapper.MapToGamePaginatedResponseAsync(queryResult, cancellationToken);
        }
    }
}