using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models.Dtos;

public class RecipeViewActivityDto
{
    public ObjectId RecipeId { get; set; }

    public DateTime LocalDateTime { get; set; }
}

