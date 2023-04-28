using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Application.MappingProfiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();

        CreateMap<ContactCreateDto, Contact>();
    }
}
