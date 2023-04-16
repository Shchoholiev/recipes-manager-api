using Microsoft.AspNetCore.Http;

namespace RecipesManagerApi.Application.IServices;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(IFormFile file, Guid guid, string fileExtension, CancellationToken cancellationToken);

    Task<string> UploadFileAsync(byte[] file, Guid guid, string fileExtension, CancellationToken cancellationToken);

    Task DeleteFileAsync(Guid guid, string fileExtension, CancellationToken cancellationToken);
}
