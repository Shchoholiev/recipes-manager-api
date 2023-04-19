using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.DtoConverter;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class MenuProfile : Profile
{
	public MenuProfile()
	{
		CreateMap<Menu, MenuDto>()
		.ForMember(dest => dest.Id,  opt => opt.MapFrom(src => src.Id.ToString()))
		.ForMember(dest => dest.Recipes, opt => opt.MapFrom<MenuRecipesIdsToRecipesResolver>());
		//CreateMap<MenuDto, Menu>()
		//.ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)));
		
		CreateMap<MenuCreateDto, Menu>();
	}
}