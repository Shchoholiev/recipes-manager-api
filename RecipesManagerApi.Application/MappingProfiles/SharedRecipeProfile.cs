using AutoMapper;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class SharedRecipeProfile : Profile
{
    public SharedRecipeProfile()
    {
        CreateMap<SharedRecipe, SharedRecipeDto>().ReverseMap();

        CreateMap<SharedRecipeCreateDto, SharedRecipe>().ReverseMap();
    }
}
