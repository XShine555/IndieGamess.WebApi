using WebApi.DataTransferObjects.GameBuild.Responses;

namespace WebApi.Mappers
{
    public interface IGameBuildMapper
    {
        GameBuildResponse MapToGameBuildResponse(global::Application.Games.Builds.Responses.ApplicationGameBuild applicationGameBuild);

        GameBuildMutationResponse MapToGameBuildMutationResponse(global::Application.Games.Builds.Responses.ApplicationGameBuildMutation applicationGameBuild);

        GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(global::Application.Games.Builds.Responses.ApplicationPreSignGameFileRequestMutation fileRequest);
    }
}
