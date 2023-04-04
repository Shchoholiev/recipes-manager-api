using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class Image : EntityBase
{
    public String OriginalPhotoLink { get; set; }

    public bool SmallPhotoLink { get; set; }

    public String Md2Hash { get; set; }
}
