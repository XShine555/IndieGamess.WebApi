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
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("users")]
    [Tags("Users")]
    public class UserEndpoint(IMediator mediator, IUserApplicationMapper userMapper, IGameApplicationMapper gameMapper)
        : Controller
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get Users")]
        [EndpointSummary("Get Users")]
        public async Task<PaginatedResponse<UserListItemResponse>> Get( [FromQuery] GetUsersRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUsersQuery(query.Username, query.PageNumber, query.PageSize), cancellationToken);
            return await userMapper.MapToUserPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet]
        [Route("{userId}", Name = "Get User By Id")]
        [EndpointSummary("Get User By Id")]
        public async Task<Result<GetUserResponse>> GetById(Guid userId, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(userId), cancellationToken);
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
        public async Task<Result> UpdateProfilePicture( [FromForm] UpdateUserProfilePictureRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var fileData = FileData.FromFormFile(request.ProfilePicture);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(currentUser.IdentityId, fileData), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpGet("me", Name = "Get Current User")]
        [EndpointSummary("Get Current User")]
        [Authorize]
        public async Task<Result<GetBasicUserResponse>> GetCurrentUser(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetBasicUserQuery(currentUser.IdentityId), cancellationToken);
            return await userMapper.MapToBasicUserResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpPut("me", Name = "Update User")]
        [EndpointSummary("Update User")]
        [Authorize]
        public async Task<Result<UpdateUserResponse>> Update( [FromForm] UpdateUserRequest request, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(currentUser.IdentityId, request.DisplayName), cancellationToken);
            return commandResult.Map(userMapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("me/collections", Name = "Add Game Collection")]
        [EndpointSummary("Add Game Collection")]
        [Authorize]
        public async Task<Result<GameCollectionResponse>> AddGameCollection( [FromBody] CreateGameCollectionRequest createGameCollectionRequest,
            CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new CreateUserGameCollectionCommand(currentUser.IdentityId, createGameCollectionRequest.Name), cancellationToken);
            return commandResult.Map(userMapper.MapToGameCollectionResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("me/collections/{collectionId}", Name = "Remove Game Collection")]
        [EndpointSummary("Remove Game Collection")]
        [Authorize]
        public async Task<Result> RemoveGameCollection(Guid collectionId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveUserGameCollectionCommand(currentUser.IdentityId, collectionId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("me/collections/{collectionId}/games/{gameId}", Name = "Add Game To Collection")]
        [EndpointSummary("Add Game To Collection")]
        [Authorize]
        public async Task<Result> AddGameToCollection(Guid collectionId, Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new AddGameToUserCollectionCommand(currentUser.IdentityId, collectionId, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpDelete("me/collections/{collectionId}/games/{gameId}", Name = "Remove Game From Collection")]
        [EndpointSummary("Remove Game From Collection")]
        [Authorize]
        public async Task<Result> RemoveGameFromCollection(Guid collectionId, Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameFromUserCollectionCommand(currentUser.IdentityId, collectionId, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("me/promote-to-developer", Name = "Promote To Developer")]
        [EndpointSummary("Promote To Developer")]
        [Authorize]
        public async Task<Result<UpdateUserResponse>> PromoteToDeveloper(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PromoteUserToDeveloperCommand(currentUser.IdentityId), cancellationToken);
            return commandResult.Map(userMapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("me/cart/{gameId}", Name = "Add Item To Cart")]
        [EndpointSummary("Add Item To Cart")]
        [Authorize]
        public async Task<Result> AddItemToCart(Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new AddGameToUserCartCommand(currentUser.IdentityId, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpDelete("me/cart/{gameId}", Name = "Remove Item From Cart")]
        [EndpointSummary("Remove Item From Cart")]
        [Authorize]
        public async Task<Result> RemoveItemFromCart(Guid gameId, CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new RemoveGameFromUserCartCommand(currentUser.IdentityId, gameId), cancellationToken);
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

        [TranslateResultToActionResult]
        [HttpPost("me/cart/checkout", Name = "Checkout Cart")]
        [EndpointSummary("Checkout Cart")]
        [Authorize]
        public async Task<Result> Checkout([FromServices] ICurrentUser currentUser, CancellationToken cancellationToken)
        {
            var cartResult = await mediator.Send(new GetUserCartItemsQuery(currentUser.IdentityId), cancellationToken);
            if (!cartResult.IsSuccess)
                return Result.Error("Error al obtener el carrito.");

            foreach (var cartItem in cartResult.Value)
            {
                await mediator.Send(new AddGameToUserLibraryCommand(currentUser.IdentityId, cartItem.Game.Id), cancellationToken);
                await mediator.Send(new RemoveGameFromUserCartCommand(currentUser.IdentityId, cartItem.Game.Id), cancellationToken);
            }

            return Result.Success();
        }

        [TranslateResultToActionResult]
        [HttpGet("me/library", Name = "Get User Library")]
        [EndpointSummary("Get User Library")]
        [Authorize]
        public async Task<Result<GetUserLibraryResponse>> GetLibrary(
            CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(currentUser.IdentityId), cancellationToken);
            return await userMapper.MapToUserLibraryResponse(queryResult, cancellationToken);
        }

        [HttpGet("me/created-games", Name = "Get Created Games")]
        [EndpointSummary("Get Created Games")]
        [Authorize]
        public async Task<PaginatedResponse<GameListItemResponse>> GetCreatedGames( [FromQuery] GetCreatedGamesRequest request, CancellationToken cancellationToken,
            [FromServices] ICurrentUser currentUser)
        {
            var queryResult = await mediator.Send(new GetGamesQuery(
                request.Title,
                null,
                request.PageNumber,
                request.PageSize,
                GameCatalogQueryMode.Developer), cancellationToken);
            return await gameMapper.MapToGameListPaginatedResponseAsync(queryResult, cancellationToken);
        }
    }
}