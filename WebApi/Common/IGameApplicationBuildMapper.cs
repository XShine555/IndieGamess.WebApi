using Application.Games.Builds.Responses;
using WebApi.DataTransferObjects.GameBuild.Responses;
using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.Common
{
    public interface IGameApplicationBuildMapper
    {
        Task<GameBuildUserResponse> MapToGameBuildUserResponse(ApplicationGameBuild applicationGameBuild, CancellationToken cancellationToken);

        Task<GameBuildDeveloperResponse> MapToGameBuildDeveloperResponse(ApplicationGameBuild applicationGameBuild, CancellationToken cancellationToken);

        GameBuildMutationResponse MapToGameBuildMutationResponse(ApplicationGameBuildMutation applicationGameBuild);

        GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(ApplicationPreSignGameFileRequestMutation fileRequest);

        GameBuildAsUserListItemResponse MapToGameBuildAsUserListItemResponse(ApplicationGameBuildListItem applicationGameBuildListItem);

        GameBuildAsDeveloperListItemResponse MapToGameBuildAsDeveloperListItemResponse(ApplicationGameBuildListItem applicationGameBuildListItem);

        Task<GameFileUserResponse> MapToGameFileUserResponse(ApplicationFileInfo applicationFileInfo, CancellationToken cancellationToken);
    }
}
