using RecipesManagerApi.Domain.Common;
using MongoDB.Bson;

namespace RecipesManagerApi.Domain.Entities;

public class SavedRecipe : EntityBase
{ 
    public ObjectId RecipeId { get; set; }
}
