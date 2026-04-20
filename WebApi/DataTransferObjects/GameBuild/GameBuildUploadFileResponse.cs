namespace WebApi.DataTransferObjects.GameBuild
{
    public record GameBuildUploadFileResponse(
        string OriginalFilePath,
        string StorageKey,
        string UploadUrl);
}
