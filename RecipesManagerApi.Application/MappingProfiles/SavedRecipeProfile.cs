using AutoMapper;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
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

