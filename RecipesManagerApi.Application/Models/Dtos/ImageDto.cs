using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Application.Models.Dtos;

public class ImageDto
{
    public string Id { get; set; }

    public Guid OriginalPhotoGuid { get; set; }

    public Guid SmallPhotoGuid { get; set; }

    public string Extension { get; set; }

    public string Md5Hash { get; set; }

    public ImageUploadStates ImageUploadState { get; set; }
}
