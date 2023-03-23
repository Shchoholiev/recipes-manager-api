using System;

namespace RecipesManagerApi.Application.IServices;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(string fileName);

    Task DeleteFileAsync(string fileName);
}
