using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class MenuProfile : Profile
{
	public MenuProfile()
	{
		CreateMap<MenuLookedUp, MenuDto>().ReverseMap();
		
		CreateMap<MenuCreateDto, Menu>().ReverseMap();
		
		CreateMap<MenuDto, Menu>()
		.ForMember(dest => dest.RecipesIds, opt => opt.MapFrom((src, dest, _, context) => 
		context.Items.TryGetValue("RecipesIds", out var recipesIds) ? recipesIds : null));
		
		CreateMap<MenuCreateDto, MenuDto>();
	}
}