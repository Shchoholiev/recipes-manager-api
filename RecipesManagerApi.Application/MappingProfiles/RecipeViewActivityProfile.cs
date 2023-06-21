using AutoMapper;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class RecipeViewActivityProfile : Profile
{
    public RecipeViewActivityProfile()
    {
        CreateMap<RecipeViewActivityDto, RecipeViewActivity>().ReverseMap();
    }
}

