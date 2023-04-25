using RecipesManagerApi.Domain.Common;
using MongoDB.Bson;

namespace RecipesManagerApi.Domain.Entities;

public class SavedRecipe : EntityBase
{
    public ObjectId UserId { get; set; }

    public ObjectId RecipeId { get; set; }

    public bool IsDeleted { get; set; }
}
