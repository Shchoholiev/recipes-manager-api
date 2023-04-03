using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));            
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)));            
            CreateMap<CategoryCreateDto, Category>();         
        }
    }
}
