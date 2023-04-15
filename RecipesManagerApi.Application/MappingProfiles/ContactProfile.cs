using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Application.MappingProfiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();

        CreateMap<ContactCreateDto, Contact>();
    }
}
