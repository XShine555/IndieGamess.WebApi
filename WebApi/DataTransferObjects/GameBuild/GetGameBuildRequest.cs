using Domain.Games.Enums;

namespace WebApi.DataTransferObjects.GameBuild
{
    public record GetGameBuildRequest(
        Guid BuildId,
        string VersionName,
        GameBuildStatus Status,
        bool IsReleaseBuild);
}