namespace WebApi.DataTransferObjects.Games
{
    public class GetQuery
    {
        public string Title { get; set; } = string.Empty;

        public Guid[] Genres { get; set; } = Array.Empty<Guid>();

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
