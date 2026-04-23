using Application.Abstractions.Common;
using Application.Games.Catalog.Responses;
using Application.Games.Media.Responses;
using Application.Genres.Responses;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.Common;
using WebApi.DataTransferObjects.Games.Responses;
using WebApi.DataTransferObjects.Genres.Responses;
using WebApi.DataTransferObjects.Users.Requests;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.Mappers
{
    public interface IUserMapper
    {
        Task<PaginatedResponse<UserListItemResponse>> MapToUserPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationUserListItem> paginatedResponse,
            CancellationToken cancellationToken);

        PaginatedResponse<GameCollectionListItemResponse> MapToGameCollectionPaginatedResponse(
            PaginatedApplicationResponse<ApplicationUserCollectionListItem> paginatedResponse);

        Task<Result<GetUserCartResponse>> MapToGetUserCartResponse(
            Result<IReadOnlyCollection<ApplicationUserCartItem>> result,
            CancellationToken cancellationToken);

        Task<GetUserCartResponse> MapToGetUserCartResponse(
            IReadOnlyCollection<ApplicationUserCartItem> cartItems,
            CancellationToken cancellationToken);

        Task<Result<UserResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken);

        Task<UserResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken);

        UpdateUserResponse MapToUpdateUserResponse(ApplicationUserMutation applicationUser);

        Task<UserListItemResponse> MapToUserListItemResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken);

        GameCollectionResponse MapToGameCollectionResponse(ApplicationUserCollectionListItem gameCollection);

        GameCollectionListItemResponse MapToGameCollectionListItemResponse(ApplicationUserCollectionListItem gameCollection);

        Task<GameCollectionDetailsResponse> MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection, CancellationToken cancellationToken);
    }
}
