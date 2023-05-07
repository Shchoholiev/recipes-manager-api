using AutoMapper;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class ShoppingListProfile : Profile
{
	public ShoppingListProfile()
	{
		CreateMap<ShoppingListCreateDto, ShoppingList>().ReverseMap();
		
		CreateMap<ShoppingListLookedUp, ShoppingListDto>().ReverseMap();
	}
}