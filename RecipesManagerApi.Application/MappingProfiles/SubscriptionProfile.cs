using AutoMapper;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<Subscription, SubscriptionDto>().ReverseMap();

        CreateMap<SubscriptionCreateDto, Subscription>().ReverseMap();
    }
}

