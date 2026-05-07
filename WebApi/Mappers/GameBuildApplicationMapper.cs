using Application.Abstractions.Storage;
using Application.Games.Builds.Responses;
using WebApi.Common;
using WebApi.DataTransferObjects.GameBuild.Responses;
using WebApi.DataTransferObjects.Games.Responses;

namespace WebApi.Mappers
{
    public class GameBuildApplicationMapper(IS3Service s3Service) : SignedUrlMapper(s3Service), IGameApplicationBuildMapper
    {
        public async Task<GameBuildUserResponse> MapToGameBuildUserResponse(ApplicationGameBuild applicationGameBuild, CancellationToken cancellationToken)
        {
            var manifestUrl = await CreateSignedUrlAsync(
                applicationGameBuild.ManifestPath!,
                cancellationToken);

            return new GameBuildUserResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                manifestUrl,
                applicationGameBuild.ExecutableFilePath,
                applicationGameBuild.IsReleaseBuild);
        }

        public async Task<GameBuildDeveloperResponse> MapToGameBuildDeveloperResponse(ApplicationGameBuild applicationGameBuild, CancellationToken cancellationToken)
        {
            string? manifestUrl = null;
            if (!string.IsNullOrEmpty(applicationGameBuild.ManifestPath))
                manifestUrl = await CreateSignedUrlAsync(
                    applicationGameBuild.ManifestPath!,
                    cancellationToken);

            return new GameBuildDeveloperResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.Status.ToString(),
                applicationGameBuild.IsReleaseBuild,
                manifestUrl,
                applicationGameBuild.ExecutableFilePath);
        }

        public GameBuildMutationResponse MapToGameBuildMutationResponse(ApplicationGameBuildMutation applicationGameBuild)
        {
            return new GameBuildMutationResponse(
                applicationGameBuild.BuildId,
                applicationGameBuild.VersionName,
                applicationGameBuild.ExecutablePath,
                applicationGameBuild.CreatedAt);
        }

        public GameBuildUploadFileResponse MapToGameBuildUploadFileResponse(ApplicationPreSignGameFileRequestMutation fileRequest)
        {
            return new GameBuildUploadFileResponse(
                fileRequest.OriginalFilePath,
                fileRequest.StorageKey,
                fileRequest.UploadUrl);
        }

        public GameBuildAsUserListItemResponse MapToGameBuildAsUserListItemResponse(ApplicationGameBuildListItem applicationGameBuildListItem)
        {
            return new GameBuildAsUserListItemResponse(
                applicationGameBuildListItem.BuildId,
                applicationGameBuildListItem.VersionName,
                applicationGameBuildListItem.IsReleaseBuild);
        }

        public GameBuildAsDeveloperListItemResponse MapToGameBuildAsDeveloperListItemResponse(ApplicationGameBuildListItem applicationGameBuildListItem)
        {
            return new GameBuildAsDeveloperListItemResponse(
                applicationGameBuildListItem.BuildId,
                applicationGameBuildListItem.VersionName,
                applicationGameBuildListItem.Status.ToString(),
                applicationGameBuildListItem.IsReleaseBuild);
        }
    }
}
