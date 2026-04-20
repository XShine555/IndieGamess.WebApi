using Domain.Games.Enums;

namespace WebApi.DataTransferObjects.GameBuild
{
    public record GameBuildResponse(
        Guid BuildId,
        string VersionName,
        GameBuildStatus Status,
        bool IsReleaseBuild,
        DateTime CreatedAt);
}
