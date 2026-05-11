using Application.Abstractions.Storage;
using Application.Games.Builds.Responses;
using Domain.Games.Entities;
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
                applicationGameBuild.ExecutableFilePath!,
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

        public async Task<GameFileUserResponse> MapToGameFileUserResponse(ApplicationFileInfo applicationFileInfo, CancellationToken cancellationToken)
        {
            string downloadUrl = await s3Service.GetSignedUrlAsync(applicationFileInfo.FilePath, TimeSpan.FromHours(1), cancellationToken);
            var buildId = applicationFileInfo.GameBuildId.ToString();
            var index = applicationFileInfo.FilePath.IndexOf(buildId);
            if (index == -1)
                throw new InvalidOperationException("File path does not contain GameBuildId.");
            var startIndex = index + buildId.Length;
            var relativePath = applicationFileInfo.FilePath.Substring(startIndex + 1);
            return new GameFileUserResponse(
                applicationFileInfo.Id,
                relativePath,
                downloadUrl,
                applicationFileInfo.Size,
                applicationFileInfo.Hash,
                applicationFileInfo.HashAlgorithm);
        }
    }
}
