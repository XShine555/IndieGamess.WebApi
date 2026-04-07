using Application.Contracts.Application;

namespace WebApi.Contracts
{
    public record PaginatedResponse<T>(
        IReadOnlyCollection<T> Items,
        int PageNumber,
        int PageSize,
        int PageCount,
        int TotalItemCount,
        bool HasNextPage,
        bool HasPreviousPage)
    {
        public static PaginatedResponse<TDestination> FromApplicationResponse<TSource, TDestination>(
                    PaginatedApplicationResponse<TSource> applicationResponse,
                    Func<TSource, TDestination> mapper)
        {
            return new PaginatedResponse<TDestination>(
                applicationResponse.Items.Select(mapper).ToArray(),
                applicationResponse.PageNumber,
                applicationResponse.PageSize,
                applicationResponse.PageCount,
                applicationResponse.TotalItemCount,
                applicationResponse.HasNextPage,
                applicationResponse.HasPreviousPage);
        }
    }
}
