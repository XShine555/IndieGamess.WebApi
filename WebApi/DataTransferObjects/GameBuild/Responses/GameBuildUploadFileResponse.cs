namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameBuildUploadFileResponse(
        string OriginalFilePath,
        string StorageKey,
        string UploadUrl);
}
