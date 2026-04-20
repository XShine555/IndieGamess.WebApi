namespace WebApi.DataTransferObjects.GameBuild
{
    public record GameBuildUploadFilesResponse(
        string OriginalFilePath,
        string StorageUrl);
}
