namespace WebApi.DataTransferObjects.GameBuild.Responses
{
    public record GameFileUserResponse(
        Guid Id,
        string RelativePath,
        string DownloadUrl,
        long Size,
        string Hash,
        string HashAlgorithm);
}
