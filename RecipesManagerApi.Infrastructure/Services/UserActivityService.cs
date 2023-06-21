using AutoMapper;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IRecipeViewActivitiesRepository _repository;

    private readonly IMapper _mapper;  

    public UserActivityService(IMapper mapper, IRecipeViewActivitiesRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<RecipeViewActivityDto> AddRecipeViewActivityAsync(RecipeViewActivityDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<RecipeViewActivity>(dto);
        entity.CreatedDateUtc = DateTime.UtcNow;
        entity.CreatedById = GlobalUser.Id.Value;
        var result = await this._repository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<RecipeViewActivityDto>(result);
    }
}

