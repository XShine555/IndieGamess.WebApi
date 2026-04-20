using Domain.Games.Enums;

namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildResponse(
        Guid BuildId,
        string VersionName,
        GameBuildStatus Status,
        bool IsReleaseBuild,
        DateTime CreatedAt);
}
