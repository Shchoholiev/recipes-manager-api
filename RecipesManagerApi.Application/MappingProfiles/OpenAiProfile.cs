using AutoMapper;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class OpenAiProfile : Profile
{
    public OpenAiProfile()
    {
        CreateMap<OpenAiLogDto, OpenAiLog>().ReverseMap();
    }
}
