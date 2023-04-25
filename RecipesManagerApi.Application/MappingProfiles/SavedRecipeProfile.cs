using AutoMapper;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class SavedRecipeProfile : Profile
{
    public SavedRecipeProfile()
    {
        CreateMap<SavedRecipe, SavedRecipeDto>().ReverseMap();

        CreateMap<SavedRecipeCreateDto, SavedRecipe>().ReverseMap();
    }
}

