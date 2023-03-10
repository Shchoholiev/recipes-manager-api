using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models;

public class ImageDto
{
    public ObjectId Id { get; set; }

    public String OriginalPhotoLink { get; set; }

}
