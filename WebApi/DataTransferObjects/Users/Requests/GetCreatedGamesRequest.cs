namespace WebApi.DataTransferObjects.Users.Requests
{
    public class GetCreatedGamesRequest
    {
        public string Title { get; set; } = string.Empty;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
