using Application.Abstractions.Common;
using System.Threading;

namespace WebApi.Common
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

        public static async Task<PaginatedResponse<TDestination>> FromApplicationResponseAsync<TSource, TDestination>(
            PaginatedApplicationResponse<TSource> applicationResponse,
            Func<TSource, Task<TDestination>> mapper,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var mappedItems = await Task.WhenAll(
                applicationResponse.Items.Select(mapper)
            );

            return new PaginatedResponse<TDestination>(
                mappedItems,
                applicationResponse.PageNumber,
                applicationResponse.PageSize,
                applicationResponse.PageCount,
                applicationResponse.TotalItemCount,
                applicationResponse.HasNextPage,
                applicationResponse.HasPreviousPage);
        }
    }
}
