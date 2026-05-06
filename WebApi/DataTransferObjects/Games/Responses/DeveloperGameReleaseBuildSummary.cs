namespace WebApi.DataTransferObjects.Games.Responses
{
    public record DeveloperGameReleaseBuildSummary(
        Guid BuildId,
        string VersionName);
}
