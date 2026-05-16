namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameBuildAsUserListItemResponse(
        Guid Id,
        string VersioName,
        bool IsReleaseBuild);
}
