using AutoMapper;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.LookUps;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeDto>().ReverseMap();
        
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Thumbnail, opt => opt.Ignore());

        CreateMap<RecipeLookUp, RecipeDto>();
    }
}
