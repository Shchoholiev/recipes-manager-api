using AutoMapper;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<LogDto, Log>().ReverseMap();
    }
}
