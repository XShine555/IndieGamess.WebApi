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
        [HttpGet(Name = "Get Admin Users")]
        [EndpointSummary("Get Admin Users")]
        public async Task<PaginatedResponse<UserListItemResponse>> Get([FromQuery] GetUsers query, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUsersQuery(query.Username, query.PageNumber, query.PageSize), cancellationToken);
            return await mapper.MapToUserPaginatedResponseAsync(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}", Name = "Get Admin User By Id")]
        [EndpointSummary("Get Admin User By Id")]
        public async Task<Result<UserResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserByIdentityIdQuery(id), cancellationToken);
            return await mapper.MapToUserResponse(queryResult, cancellationToken);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}/collections", Name = "Get Admin User Collections")]
        [EndpointSummary("Get Admin User Collections")]
        public async Task<PaginatedResponse<GameCollectionListItemResponse>> GetCollections(
            Guid id,
            [FromQuery] GetUserCollections query,
            CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserCollectionsQuery(id, query.PageNumber, query.PageSize), cancellationToken);
            return mapper.MapToGameCollectionPaginatedResponse(queryResult);
        }

        [TranslateResultToActionResult]
        [HttpGet("{id}/collections/{collectionId}", Name = "Get Admin User Collection By Id")]
        [EndpointSummary("Get Admin User Collection By Id")]
        public async Task<Result<GameCollectionDetailsResponse>> GetCollectionById(Guid id, Guid collectionId, CancellationToken cancellationToken)
        {
            var queryResult = await mediator.Send(new GetUserCollectionByIdQuery(id, collectionId), cancellationToken);
            return queryResult.Map(mapper.MapToGameCollectionDetailsResponse);
        }

        [TranslateResultToActionResult]
        [HttpPatch("{id}/profile-picture", Name = "Update Admin Profile Picture")]
        [EndpointSummary("Update Admin Profile Picture")]
        public async Task<Result> UpdateProfilePicture(Guid id, [FromForm] IFormFile formFile, CancellationToken cancellationToken)
        {
            var fileData = FileData.FromFormFile(formFile);
            var commandResult = await mediator.Send(new UpdateUserProfilePictureCommand(id, fileData), cancellationToken);
            return commandResult;
        }

        [TranslateResultToActionResult]
        [HttpPut("{id}", Name = "Update Admin User")]
        [EndpointSummary("Update Admin User")]
        public async Task<Result<UpdateUserResponse>> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new UpdateUserCommand(id, request.DisplayName), cancellationToken);
            return commandResult.Map(mapper.MapToUpdateUserResponse);
        }

        [TranslateResultToActionResult]
        [HttpPost("{id}/collections", Name = "Add Admin Game Collection")]
        [EndpointSummary("Add Admin Game Collection")]
        public async Task<Result<GameCollectionResponse>> AddGameCollection(Guid id, [FromBody] CreateGameCollectionRequest createGameCollectionRequest,
            CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new CreateUserGameCollectionCommand(id, createGameCollectionRequest.Name), cancellationToken);
            return commandResult.Map(mapper.MapToGameCollectionResponse);
        }

        [TranslateResultToActionResult]
        [HttpDelete("{id}/collections/{collectionId}", Name = "Remove Admin Game Collection")]
        [EndpointSummary("Remove Admin Game Collection")]
        public async Task<Result> RemoveGameCollection(Guid id, Guid collectionId, CancellationToken cancellationToken)
        {
            var commandResult = await mediator.Send(new RemoveUserGameCollectionCommand(id, collectionId), cancellationToken);
            return commandResult;
        }
    }
}
