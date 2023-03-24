using System;

namespace RecipesManagerApi.Application.IServices;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(string fileName, CancellationToken cancellationToken);

    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
}
