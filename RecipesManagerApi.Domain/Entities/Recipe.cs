using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class Recipe : EntityBase
{
    public string Name { get; set; }

    public Image? Thumbnail { get; set; }

    public string? Text { get; set; }

    public List<Ingredient>? Ingredients { get; set; }

    public string? IngredientsText { get; set; }

    public List<Category> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }

    public int? MinutesToCook { get; set; }

    public bool IsPublic { get; set; }
}
