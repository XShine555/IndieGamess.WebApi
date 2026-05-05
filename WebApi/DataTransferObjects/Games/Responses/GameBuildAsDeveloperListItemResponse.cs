namespace WebApi.DataTransferObjects.Games.Responses
{
    public record GameBuildAsDeveloperListItemResponse(
        Guid Id,
        string VersionName,
        string Status,
        bool IsReleaseBuild);
}
