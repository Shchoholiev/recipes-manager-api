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
		CreateMap<MenuLookedUp, MenuDto>()
		.ForMember(dest => dest.SentTo, opt => opt.MapFrom(src => src.SentToContacts));
		
		CreateMap<MenuDto, MenuLookedUp>()
		.ForMember(dest => dest.SentToContacts, opt => opt.MapFrom(src => src.SentTo));
		
		CreateMap<MenuCreateDto, Menu>().ReverseMap();
	}
}