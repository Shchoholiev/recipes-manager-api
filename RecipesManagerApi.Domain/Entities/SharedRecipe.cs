using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class SharedRecipe : EntityBase
{
    public ObjectId RecipeId { get; set; }

    public int VisitsCount { get; set;}
}
