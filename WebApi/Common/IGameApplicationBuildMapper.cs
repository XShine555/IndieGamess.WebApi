using Application.Games.Builds.Responses;
using WebApi.DataTransferObjects.GameBuild.Responses;

namespace WebApi.Common
{
    public interface IGameApplicationBuildMapper
    {
        GameBuildResponse MapToGameBuildResponse(ApplicationGameBuild applicationGameBuild);

        GameBuildMutationResponse MapToGameBuildMutationResponse(ApplicationGameBuildMutation applicationGameBuild);

        GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(ApplicationPreSignGameFileRequestMutation fileRequest);
    }
}
