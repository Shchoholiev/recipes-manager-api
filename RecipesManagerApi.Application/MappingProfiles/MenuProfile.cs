using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class MenuProfile : Profile
{
	public MenuProfile()
	{
		CreateMap<Menu, MenuDto>()
		.ForMember(dest => dest.Id,  opt => opt.MapFrom(src => src.Id.ToString()))
		.ForMember(dest => dest.Recipes, opt => opt.MapFrom((src, dest, _, context) => 
		context.Items.TryGetValue("Recipes", out var recipes) ? recipes : null));
		CreateMap<MenuDto, Menu>()
		.ForMember(dest => dest.Id,  opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
		.ForMember(dest => dest.RecipesIds, opt => opt.MapFrom((src, dest, _, context) => 
		context.Items.TryGetValue("RecipesIds", out var recipesIds) ? recipesIds : null));
		
		CreateMap<MenuCreateDto, Menu>()
		.ForMember(dest => dest.RecipesIds, opt => opt.MapFrom(src => src.RecipesIds.Select(x => x.ToString()).ToList()));
		
		CreateMap<MenuCreateDto, MenuDto>();
	}
}