namespace WebApi.DataTransferObjects.GameBuild
{
    public record GameBuildMutationResponse(
        Guid BuildId,
        string VersionName,
        DateTime CreatedAt);
}
