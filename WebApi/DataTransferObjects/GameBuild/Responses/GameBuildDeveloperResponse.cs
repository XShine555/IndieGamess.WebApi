namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildDeveloperResponse(
        Guid BuildId,
        string VersionName,
        string Status,
        bool IsReleaseBuild,
        string? ManifestUrl);
}
