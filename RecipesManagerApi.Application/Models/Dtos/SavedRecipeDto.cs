using System;

namespace RecipesManagerApi.Application.Models.Dtos;

public class SavedRecipeDto
{
    public string Id { get; set; }

    public string RecipeId { get; set; }

    public string UserId { get; set; }
}

