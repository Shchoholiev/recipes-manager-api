using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.GlodalInstances;

namespace RecipesManagerApi.Infrastructure.Services;

public class SavedRecipesService : ISavedRecipesService
{
    private readonly IMapper _mapper;

    private readonly ISavedRecipesRepository _repository;

    public SavedRecipesService(IMapper mapper, ISavedRecipesRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<SavedRecipeDto> AddSavedRecipeAsync(SavedRecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<SavedRecipe>(dto);
        entity.CreatedById = (ObjectId)GlobalUser.Id;
        entity.CreatedDateUtc = DateTime.UtcNow;
        await this._repository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<SavedRecipeDto>(entity);
    }

    public async Task<OperationDetails> DeleteSavedRecipeAsync(SavedRecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<SavedRecipe>(dto);
        entity.IsDeleted = true;
        await this._repository.UpdateSavedRecipeAsync(entity, cancellationToken);
        return new OperationDetails() { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
    }

    public async Task<SavedRecipeDto> GetSavedRecipeAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetSavedRecipeAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<SavedRecipe>();
        }
        if (entity.IsDeleted == true)
        {
            throw new EntityIsDeletedException<SavedRecipe>();
        }
        return this._mapper.Map<SavedRecipeDto>(entity);
    }

    public async Task<PagedList<SavedRecipeDto>> GetSavedRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, x => x.IsDeleted == false, cancellationToken);
        var dtos = this._mapper.Map<List<SavedRecipeDto>>(entities);
        var count = await this._repository.GetTotalCountAsync(x => x.IsDeleted == false);
        return new PagedList<SavedRecipeDto>(dtos, pageNumber, pageSize, count);
    }
}

