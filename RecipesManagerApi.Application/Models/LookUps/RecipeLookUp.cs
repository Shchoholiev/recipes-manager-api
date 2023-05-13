using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.Models.LookUps;

public class RecipeLookUp
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public Image? Thumbnail { get; set; }

    public List<Ingredient>? Ingredients { get; set; }

    public string? Text { get; set; }

    public string? IngredientsText { get; set; }

    public List<Category> Categories { get; set; }

    public int? Calories { get; set; }

    public int? ServingsCount { get; set; }

    public bool IsPublic { get; set; }

    public bool IsSaved { get; set; }

    public bool IsDeleted { get; set; }

    public ObjectId? LastModifiedById { get; set; }

    public DateTime? LastModifiedDateUtc { get; set; }

    public ObjectId CreatedById { get; set; }

    public User CreatedBy { get; set; }

    public DateTime CreatedDateUtc { get; set; }
}
