using WebApi.Common;
using WebApi.DataTransferObjects.GameBuild.Responses;

namespace WebApi.Mappers
{
    public class GameBuildApplicationMapper : IGameApplicationBuildMapper
    {
        public GameBuildResponse MapToGameBuildResponse(global::Application.Games.Builds.Responses.ApplicationGameBuild applicationGameBuild)
        {
            return new GameBuildResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.Status,
                applicationGameBuild.IsReleaseBuild,
                applicationGameBuild.CreatedAt);
        }

        public GameBuildMutationResponse MapToGameBuildMutationResponse(global::Application.Games.Builds.Responses.ApplicationGameBuildMutation applicationGameBuild)
        {
            return new GameBuildMutationResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.CreatedAt);
        }

        public GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(global::Application.Games.Builds.Responses.ApplicationPreSignGameFileRequestMutation fileRequest)
        {
            return new GameBuildUploadFileResponse(
                fileRequest.OriginalFilePath,
                fileRequest.StorageKey,
                fileRequest.UploadUrl);
        }
    }
}
