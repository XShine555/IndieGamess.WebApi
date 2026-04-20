namespace WebApi.DataTransferObjects.GameBuild
{
    public record GameBuildMutationResponse(
        Guid BuildId,
        Guid GameId,
        string VersionName);
}
