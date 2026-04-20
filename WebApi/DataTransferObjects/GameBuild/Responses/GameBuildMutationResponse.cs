namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildMutationResponse(
        Guid BuildId,
        string VersionName,
        DateTime CreatedAt);
}
