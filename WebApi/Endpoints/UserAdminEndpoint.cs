using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.DataTransferObjects.AdminUser;
using WebApi.Mappers;
using WebApi.Services;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("admin/user")]
    [Tags("Admin User")]
    [Authorize]
    public class UserAdminEndpoint(IMediator mediator, ILogger<UserAdminEndpoint> logger, AdminUserMapper mapper)
        : ControllerBase
    {
        [TranslateResultToActionResult]
        [HttpGet(Name = "Get User Admins")]
        [EndpointSummary("Get Users")]
        public async Task<PaginatedResponse<UserListItemAdminResponse>> Get([FromQuery] GetUsersAdminRequest query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUsersQuery(query.Username, query.PageNumber, query.PageSize), cancellationToken);
            return await mapper.MapToUserPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get User By Id Admin")]
        [EndpointSummary("Get User By Id")]
        public async Task<Result<UserAdminResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);
            return await mapper.MapToUserResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}/collections", Name = "Get User Collection Admins")]
        [EndpointSummary("Get User Collections")]
        public async Task<PaginatedResponse<GameCollectionListItemResponse>> GetCollections(
            Guid id,
            [FromQuery] GetUserCollectionsAdminRequest query,
            CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserCollectionsQuery(id, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGameCollectionPaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}/collections/{collectionId}", Name = "Get User Collection By Id Admin")]
        [EndpointSummary("Get User Collection By Id")]
        public async Task<Result<GameCollectionDetailsAdminResponse>> GetCollectionById(Guid id, Guid collectionId, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserCollectionByIdQuery(id, collectionId), cancellationToken);
            return queryResult.Map(mapper.MapToGameCollectionDetailsResponse);
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/profile-picture", Name = "Update Profile Picture Admin")]
        [EndpointSummary("Update Profile Picture")]
        public async Task<Result> UpdateProfilePicture(Guid id, [FromForm] IFormFile formFile, CancellationToken cancellationToken)
        {
            var fileData = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(id, fileData), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update User Admin")]
        [EndpointSummary("Update User")]
        public async Task<Result<UpdateUserAdminResponse>> Update(Guid id, [FromBody] UpdateUserAdminRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(id, request.DisplayName), cancellationToken);
            return commandResult.Map(mapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/collections", Name = "Add Game Collection Admin")]
        [EndpointSummary("Add Game Collection")]
        public async Task<Result<GameCollectionAdminResponse>> AddGameCollection(Guid id, [FromBody] CreateGameCollectionAdminRequest createGameCollectionRequest,
            CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new CreateUserGameCollectionCommand(id, createGameCollectionRequest.Name), cancellationToken);
            return commandResult.Map(mapper.MapToGameCollectionResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/collections/{collectionId}", Name = "Remove Game Collection Admin")]
        [EndpointSummary("Remove Game Collection")]
        public async Task<Result> RemoveGameCollection(Guid id, Guid collectionId, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new RemoveUserGameCollectionCommand(id, collectionId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPost("me/promote-to-devoloper", Name = "Promote To Developer")]
        [EndpointSummary("Promote To Developer")]
        [Authorize]
        public async Task<Result<UpdateUserAdminResponse>> PromoteToDeveloper(CancellationToken cancellationToken, [FromServices] ICurrentUser currentUser)
        {
            var commandResult = await mediator.Send(new PromoteUserToDeveloperCommand(currentUser.IdentityId), cancellationToken);
            return commandResult.Map(mapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/cart/{gameId}", Name = "Add Item To Cart")]
        [EndpointSummary("Add Item To Cart")]
        [Authorize]
        public async Task<Result> AddItemToCart(Guid id, Guid gameId, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new AddGameToUserCartCommand(id, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/cart/{gameId}", Name = "Remove Item From Cart")]
        [EndpointSummary("Remove Item From Cart")]
        [Authorize]
        public async Task<Result> RemoveItemFromCart(Guid id, Guid gameId, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new RemoveGameFromUserCartCommand(id, gameId), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}/cart", Name = "Get User Cart Admin")]
        [EndpointSummary("Get User Cart")]
        [Authorize]
        public async Task<Result<WebApi.DataTransferObjects.Users.GetUserCartResponse>> GetCart(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserCartItemsQuery(id), cancellationToken);
            return await queryResult.MapAsync(r => mapper.MapToGetUserCartResponse(r, cancellationToken));
        }
    }
}
