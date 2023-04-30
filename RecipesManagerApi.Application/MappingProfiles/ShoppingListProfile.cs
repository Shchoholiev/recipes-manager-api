using AutoMapper;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class ShoppingListProfile : Profile
{
	public ShoppingListProfile()
	{
		CreateMap<ShoppingListCreateDto, ShoppingList>();
		
		CreateMap<ShoppingListLookedUp, ShoppingListDto>();
		
		CreateMap<ShoppingListDto, ShoppingList>()
		.ForMember(dest => dest.RecipesIds, opt => opt.MapFrom((src, dest, _, context) => 
		context.Items.TryGetValue("RecipesIds", out var recipesIds) ? recipesIds : null));
	}
}