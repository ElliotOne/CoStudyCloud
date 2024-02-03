namespace CoStudyCloud.Infrastructure.CloudStorage
{
    /// <summary>
    /// Represents Cloud Storage interface
    /// </summary>
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile formFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}
