using Application.Games.Responses;
using WebApi.DataTransferObjects.GameBuild;

namespace WebApi.Mappers
{
    public class GameBuildMapper
    {
        public GameBuildResponse MapToGameBuildResponse(ApplicationGameBuild applicationGameBuild)
        {
            return new GameBuildResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.Status,
                applicationGameBuild.IsReleaseBuild,
                applicationGameBuild.CreatedAt);
        }

        public GameBuildMutationResponse MapToGameBuildMutationResponse(ApplicationGameBuildMutation applicationGameBuild)
        {
            return new GameBuildMutationResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.CreatedAt);
        }

        public GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(ApplicationPreSignGameFileRequestMutation fileRequest)
        {
            return new GameBuildUploadFileResponse(
                fileRequest.OriginalFilePath,
                fileRequest.StorageKey,
                fileRequest.UploadUrl);
        }
    }
}
