using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class Recipe : EntityBase
{
    public String Name { get; set; }

    public Object? Thumbnail { get; set; }

    public String? Text { get; set; }

    public List<Ingredient>? Ingredients { get; set; }

    public String? IngredientsText { get; set; }

    public List<Category> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }

    public bool IsPublic { get; set; }

    public bool IsDeleted { get; set; }

    public ObjectId? LastModifiedById { get; set; }

    public DateTime? LastModifiedDateUtc { get; set; }
}
