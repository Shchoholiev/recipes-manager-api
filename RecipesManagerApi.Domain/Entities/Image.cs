using RecipesManagerApi.Domain.Common;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Domain.Entities;

public class Image : EntityBase
{
    public Guid OriginalPhotoGuid { get; set; }

    public Guid SmallPhotoGuid { get; set; }

    public string Extension { get; set; }

    public string Md5Hash { get; set; }
    
    public ImageUploadStates ImageUploadState { get; set; }
}
