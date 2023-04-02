using System;
using Microsoft.AspNetCore.Http;

namespace RecipesManagerApi.Application.IServices;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);

    Task DeleteFileAsync(Guid guid, CancellationToken cancellationToken);
}
