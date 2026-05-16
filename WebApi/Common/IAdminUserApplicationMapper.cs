using Application.Abstractions.Common;
using Application.Users.Responses;
using Ardalis.Result;
using WebApi.DataTransferObjects.AdminUser.Responses;
using WebApi.DataTransferObjects.Users.Responses;

namespace WebApi.Common
{
    public interface IAdminUserApplicationMapper
    {
        Task<PaginatedResponse<UserListItemAdminResponse>> MapToUserPaginatedResponseAsync(
            PaginatedApplicationResponse<ApplicationUserListItem> paginatedResponse,
            CancellationToken cancellationToken);

        PaginatedResponse<GameCollectionListItemAdminResponse> MapToGameCollectionPaginatedResponse(
            PaginatedApplicationResponse<ApplicationUserCollectionListItem> paginatedResponse);

        Task<Result<GetUserCartResponse>> MapToGetUserCartResponse(
            Result<IReadOnlyCollection<ApplicationUserCartItem>> result,
            CancellationToken cancellationToken);

        Task<GetUserCartResponse> MapToGetUserCartResponse(
            IReadOnlyCollection<ApplicationUserCartItem> cartItems,
            CancellationToken cancellationToken);

        Task<Result<UserAdminResponse>> MapToUserResponse(Result<ApplicationUser> result, CancellationToken cancellationToken);

        Task<UserAdminResponse> MapToUserResponse(ApplicationUser applicationUser, CancellationToken cancellationToken);

        UpdateUserAdminResponse MapToUpdateUserResponse(ApplicationUserMutation applicationUser);

        Task<UserListItemAdminResponse> MapToUserListItemResponse(ApplicationUserListItem applicationUser, CancellationToken cancellationToken);

        GameCollectionAdminResponse MapToGameCollectionResponse(ApplicationUserCollectionListItem gameCollection);

        GameCollectionListItemAdminResponse MapToGameCollectionListItemResponse(ApplicationUserCollectionListItem gameCollection);

        Task<GameCollectionDetailsAdminResponse> MapToGameCollectionDetailsResponse(ApplicationUserCollectionDetails gameCollection, CancellationToken cancellationToken);
    }
}
