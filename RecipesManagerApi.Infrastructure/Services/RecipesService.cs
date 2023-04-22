using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Infrastructure.Services;

public class RecipesService : IRecipesService
{
    private readonly IRecipesRepository _recipesRepository;

    private readonly IMapper _mapper;

    private readonly IImagesService _imagesService;

    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public RecipesService(
        IMapper mapper, 
        IRecipesRepository recipesRepository,
        IImagesService imagesService,
        ISubscriptionsRepository subscriptionsRepository)
    {
        this._mapper = mapper;
        this._recipesRepository = recipesRepository;
        this._imagesService = imagesService;
        this._subscriptionsRepository = subscriptionsRepository;
    }

    public async Task AddRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);
        var recipe = await this._recipesRepository.AddAsync(entity, cancellationToken);
        if (dto.Thumbnail != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await dto.Thumbnail.CopyToAsync(memoryStream, cancellationToken);
                var extension = System.IO.Path.GetExtension(dto.Thumbnail.FileName).Substring(1).ToLower();
                Task.Run(() => _imagesService.AddRecipeImageAsync(memoryStream.ToArray(), extension, recipe.Id, cancellationToken));
            }
        }
    }

    public async Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, string searchString, RecipesSearchTypes? recipeSearchType, ObjectId userId, CancellationToken cancellationToken)
    {
        List<Recipe>? entities;
        List<RecipeDto>? dtos;
        int count;
        switch (recipeSearchType)
        {
            case RecipesSearchTypes.Personal:
                entities = await this.GetPersonalRecipesPageAsync(pageNumber, pageSize, searchString, userId, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x=> x.IsPublic != true && x.CreatedById == userId
                    && x.Name.Contains(searchString) || x.Categories.Exists(c => c.Name.Contains(searchString))
                    || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString));
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Public:
                entities = await this.GetPublicRecipesPageAsync(pageNumber, pageSize, searchString, userId, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x => x.IsPublic == true && x.CreatedById != userId
                    && x.Name.Contains(searchString) || x.Categories.Exists(c => c.Name.Contains(searchString))
                    || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString));
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Subscribed:
                entities = await this.GetSubscribedRecipesPageAsync(pageNumber, pageSize, searchString, userId, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetSubscriptionsCountAsync(userId, x => x.Name.Contains(searchString)
                    || x.Categories.Exists(c => c.Name.Contains(searchString)) || x.Text != null && x.Text.Contains(searchString)
                    || x.IngredientsText != null && x.IngredientsText.Contains(searchString), cancellationToken);
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Saved:
                throw new NotImplementedException();
                break;

            default:
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

    private async Task<List<Recipe>> GetPublicRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, CancellationToken cancellationToken){
        return await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsPublic == true && x.CreatedById != userId
            && x.Name.Contains(searchString) || x.Categories.Exists(c => c.Name.Contains(searchString))
            || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString), cancellationToken);
    }
    
    private async Task<List<Recipe>> GetPersonalRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, CancellationToken cancellationToken){
        return await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsPublic != true && x.CreatedById == userId
            && x.Name.Contains(searchString) || x.Categories.Exists(c => c.Name.Contains(searchString))
            || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString), cancellationToken);
    }

    private async Task<List<Recipe>> GetSubscribedRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetSubscribedRecipesAsync(userId, x=> x.Name.Contains(searchString)
            || x.Categories.Exists(c => c.Name.Contains(searchString)) || x.Text != null && x.Text.Contains(searchString)
            || x.IngredientsText != null && x.IngredientsText.Contains(searchString), cancellationToken);
    }

    private async Task<List<Recipe>> GetSavedRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsPublic != true && x.CreatedById == userId
            && x.Name.Contains(searchString) || x.Categories.Exists(c => c.Name.Contains(searchString))
            || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString), cancellationToken);
    }
}
