using Application.Abstractions.Common;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.DataTransferObjects.Users.Requests;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.Common
{
    public interface IUserApplicationMapper
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

        Task<Result<GetUserResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken);

        Task<Result<GetBasicUserResponse>> MapToBasicUserResponse(Result<ApplicationBasicUser> result, CancellationToken cancellationToken);

        Task<GetUserResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken);

        UpdateUserResponse MapToUpdateUserResponse(ApplicationUserMutation applicationUser);

        Task<UserListItemResponse> MapToUserListItemResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken);

        GameCollectionResponse MapToGameCollectionResponse(ApplicationUserCollectionListItem gameCollection);

        GameCollectionListItemResponse MapToGameCollectionListItemResponse(ApplicationUserCollectionListItem gameCollection);

        Task<GameCollectionDetailsResponse> MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection, CancellationToken cancellationToken);
    }
}
