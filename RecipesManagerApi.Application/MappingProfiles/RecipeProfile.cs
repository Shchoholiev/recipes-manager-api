using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<Recipe, RecipeDto>().ReverseMap();
        
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Thumbnail, opt => opt.Ignore());
    }
}
