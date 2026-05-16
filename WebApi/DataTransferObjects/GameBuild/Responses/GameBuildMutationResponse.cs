namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildMutationResponse(
        Guid BuildId,
        string VersionName,
        string? ExecutableFilePath,
        DateTime CreatedAt);
}
