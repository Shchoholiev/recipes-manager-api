using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;
public class RecipeViewActivity : EntityBase
{
    public ObjectId RecipeId { get; set; }

    public DateTime LocalDateTime { get; set; }
}
