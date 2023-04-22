using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class SharedRecipesService : ISharedRecipesService
{
    private readonly IMapper _mapper;

    private readonly ISharedRecipesRepository _repository;

    public SharedRecipesService(IMapper mapper, ISharedRecipesRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<SharedRecipeDto> AccessSharedRecipeAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }
        var entity = await this._repository.GetSharedRecipeAsync(objectId, cancellationToken);
        entity.VisitsCount++;
        await this._repository.UpdateSharedRecipeAsync(entity, cancellationToken);
        var result = this._mapper.Map<SharedRecipeDto>(entity);
        return result;
    }

    public async Task<SharedRecipeDto> AddSharedRecipeAsync(SharedRecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<SharedRecipe>(dto);
        await this._repository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<SharedRecipeDto>(entity);
    }

    public async Task<OperationDetails> DeleteSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<SharedRecipe>(dto);
        entity.IsDeleted = true;
        await this._repository.UpdateSharedRecipeAsync(entity, cancellationToken);
        return new OperationDetails() { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
    }

    public async Task<SharedRecipeDto> GetSharedRecipeAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetSharedRecipeAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<Role>();
        }
        return this._mapper.Map<SharedRecipeDto>(entity);
    }

    public async Task<SharedRecipeDto> UpdateSharedRecipeAsync(SharedRecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<SharedRecipe>(dto);
        await this._repository.UpdateSharedRecipeAsync(entity, cancellationToken);
        return this._mapper.Map<SharedRecipeDto>(entity);
    }
}