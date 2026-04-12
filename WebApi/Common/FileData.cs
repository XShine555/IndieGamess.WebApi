using Application.Abstractions.Common;

namespace WebApi.Common
{
    public sealed class FileData(
        string fileName,
        string fileExtension,
        string contentType,
        Stream fileStream)
        : IFileData
    {
        public string FileName => fileName;

        public string FileExtension => fileExtension;

        public string ContentType => contentType;

        public Stream FileStream => fileStream;

        public static IFileData FromFormFile(IFormFile formFile)
        {
            return new FileData(
                formFile.FileName,
                Path.GetExtension(formFile.FileName),
                formFile.ContentType,
                formFile.OpenReadStream());
        }
    }
}
