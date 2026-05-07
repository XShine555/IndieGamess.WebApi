namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildUserResponse(
        Guid BuildId,
        string VersionName,
        string ManifestUrl,
        string ExecutableFilePath,
        bool IsReleaseBuild);
}