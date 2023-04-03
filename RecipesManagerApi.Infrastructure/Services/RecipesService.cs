using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class RecipesService : IRecipesService
{
    private readonly IRecipesRepository _recipesRepository;
    private readonly IMapper _mapper;

    public RecipesService(IMapper mapper, IRecipesRepository recipesRepository)
    {
        this._mapper = mapper;
        this._recipesRepository = recipesRepository;
    }

    public async Task AddRecipeAsync(RecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);
        await this._recipesRepository.AddAsync(entity, cancellationToken);
    }

    public async Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, RecipesSearchTypes recipeSearchType, ObjectId userId, CancellationToken cancellationToken)
    {
        List<Recipe>? entities;
        List<RecipeDto>? dtos;
        int count;
        switch (recipeSearchType)
        {
            case RecipesSearchTypes.Personal:
                entities = await this.GetPersonalRecipesPageAsync(pageNumber, pageSize, userId, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x=> x.IsPublic != true && x.CreatedById == userId);
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Public:
                entities = await this.GetPublicRecipesPageAsync(pageNumber, pageSize, userId, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x=> x.IsPublic == true && x.CreatedById != userId);
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Subscribed:
                throw new NotImplementedException();
                break;

            default:
                throw new NotImplementedException();
                break;
        }
        throw new NotImplementedException();
    }

    public async Task<RecipeDto> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var entity = await this._recipesRepository.GetRecipeAsync(id, cancellationToken);
        return this._mapper.Map<RecipeDto>(entity);
    }

    public async Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);
        await this._recipesRepository.UpdateRecipeAsync(entity, cancellationToken);
    }

    private async Task<List<Recipe>> GetPublicRecipesPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken){
        return await this._recipesRepository.GetPageAsync(pageNumber,  pageSize, x => x.IsPublic == true && x.CreatedById != userId , cancellationToken);
    }
    
    private async Task<List<Recipe>> GetPersonalRecipesPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken){
        return await this._recipesRepository.GetPageAsync(pageNumber,  pageSize, x => x.IsPublic != true && x.CreatedById == userId , cancellationToken);
    }
}
