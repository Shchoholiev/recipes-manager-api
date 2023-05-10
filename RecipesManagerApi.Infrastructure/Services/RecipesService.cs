using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Enums;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.Models.Dtos;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Services;

public class RecipesService : IRecipesService
{
    private readonly IRecipesRepository _recipesRepository;

    private readonly IMapper _mapper;

    private readonly IImagesService _imagesService;

    private readonly ISubscriptionsRepository _subscriptionsRepository;

    private readonly ISavedRecipesRepository _savedRecipesRepository;

    private readonly ICategoriesRepository _categoriesRepository;

    public RecipesService(
        IMapper mapper,
        IRecipesRepository recipesRepository,
        IImagesService imagesService,
        ISubscriptionsRepository subscriptionsRepository,
        ISavedRecipesRepository savedRecipesRepository,
        ICategoriesRepository categoriesRepository)
    {
        this._mapper = mapper;
        this._recipesRepository = recipesRepository;
        this._imagesService = imagesService;
        this._savedRecipesRepository = savedRecipesRepository;
        this._subscriptionsRepository = subscriptionsRepository;
        this._categoriesRepository = categoriesRepository;
    }

    public async Task AddRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);

        if(GlobalUser.Id.Value != null)
        {
            entity.CreatedById = GlobalUser.Id.Value;
            entity.CreatedDateUtc = DateTime.UtcNow;
        }

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

    public async Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, string searchString, string? authorsId,
        List<string>? categoriesIds, RecipesSearchTypes? recipeSearchType, CancellationToken cancellationToken)
    {
        List<Recipe>? entities;
        List<RecipeDto>? dtos;
        List<Category> filterList = new List<Category>();

        foreach (var c in categoriesIds)
        {
            if (!ObjectId.TryParse(c, out var id))
            {
                throw new InvalidDataException("Provided id is invalid.");
            }
            var category = await this._categoriesRepository.GetCategoryAsync(id, cancellationToken);
            if (category != null)
            {
                filterList.Add(category);
            }
        }

        if (!ObjectId.TryParse(authorsId, out var authorId) && authorsId != string.Empty)
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var userId = GlobalUser.Id.Value;

        int count;

        switch (recipeSearchType)
        {
            case RecipesSearchTypes.Personal:
                entities = await this.GetPersonalRecipesPageAsync(pageNumber, pageSize, searchString, userId, filterList, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x => x.IsPublic != true && x.CreatedById == userId
                    && (x.Name.Contains(searchString) || x.Categories.Any(c => c.Name.Contains(searchString))
                    || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString))
                    && x.CreatedById == authorId && x.Categories.Any(c => filterList.Contains(c)));
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Public:
                entities = await this.GetPublicRecipesPageAsync(pageNumber, pageSize, searchString, userId, authorId, filterList, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x => x.IsPublic == true && x.CreatedById != userId
                    && (x.Name.Contains(searchString) || x.Categories.Any(c => c.Name.Contains(searchString)) || x.Categories.Any(c => filterList.Contains(c))
                    || x.Text != null && x.Text.Contains(searchString) || x.IngredientsText != null && x.IngredientsText.Contains(searchString)
                    || x.CreatedById == authorId));
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Subscribed:
                var subscriptions = await this._subscriptionsRepository.GetUsersSubscriptionsAsync(userId, cancellationToken);
                entities = await this.GetSubscribedRecipesPageAsync(pageNumber, pageSize, subscriptions, searchString, userId, authorId, filterList, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetSubscriptionsCountAsync(x => x.Name.Contains(searchString)
                    || x.Categories.Any(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
                    || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
                    || x.Categories.Any(c => filterList.Contains(c)), subscriptions, cancellationToken);
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            case RecipesSearchTypes.Saved:
                var saves = await this._savedRecipesRepository.GetUsersSavesAsync(userId, cancellationToken);
                entities = await this.GetSavedRecipesPageAsync(pageNumber, pageSize, saves, searchString, userId, authorId, filterList, cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetSavedRecipesCountAsync(x => x.Name.Contains(searchString)
                    || x.Categories.Any(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
                    || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
                    || x.Categories.Any(c => filterList.Contains(c)) || x.CreatedById == authorId, saves, cancellationToken);
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

            default:
                entities = await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.Name.Contains(searchString)
                    || x.Categories.Exists(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
                    || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
                    && x.CreatedById == authorId || x.Categories.Any(c => filterList.Contains(c)), cancellationToken);
                dtos = this._mapper.Map<List<RecipeDto>>(entities);
                count = await this._recipesRepository.GetTotalCountAsync(x => x.Name.Contains(searchString)
                    || x.Categories.Exists(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
                    || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
                    && x.CreatedById == authorId || x.Categories.Any(c => filterList.Contains(c)));
                return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
        }
        throw new NotImplementedException();
    }

    public async Task<RecipeDto> GetRecipeAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }
        var entity = await this._recipesRepository.GetRecipeAsync(objectId, cancellationToken);
        return this._mapper.Map<RecipeDto>(entity);
    }

    public async Task UpdateRecipeAsync(RecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);

        if (GlobalUser.Id != null)
        {
            entity.LastModifiedById = GlobalUser.Id.Value;
            entity.LastModifiedDateUtc = DateTime.UtcNow;
        }

        await this._recipesRepository.UpdateRecipeAsync(entity, cancellationToken);
    }

    private async Task<List<Recipe>> GetPublicRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, ObjectId? authorId,
        List<Category>? filterList, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsPublic == true && x.CreatedById != userId
            && (x.Name.Contains(searchString) || x.Categories.Any(c => c.Name.Contains(searchString)) || x.Categories.Any(c => filterList.Contains(c))
            || (x.Text != null && x.Text.Contains(searchString)) || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
            || x.CreatedById == authorId), cancellationToken);
    }

    private async Task<List<Recipe>> GetPersonalRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId,
        List<Category>? filterList, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsPublic != true && x.CreatedById == userId
            && (x.Name.Contains(searchString) || x.Categories.Any(c => c.Name.Contains(searchString)) || x.Categories.Any(c => filterList.Contains(c))
            || (x.Text != null && x.Text.Contains(searchString)) || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))), cancellationToken);
    }

    private async Task<List<Recipe>> GetSubscribedRecipesPageAsync(int pageNumber, int pageSize, List<Subscription> subscriptions, string searchString, ObjectId userId, ObjectId? authorId,
        List<Category>? filterList, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetSubscribedRecipesAsync(pageNumber, pageSize, subscriptions, x => x.Name.Contains(searchString)
            || x.Categories.Any(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
            || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
            || x.Categories.Any(c => filterList.Contains(c)), cancellationToken);
    }


    private async Task<List<Recipe>> GetSavedRecipesPageAsync(int pageNumber, int pageSize, List<SavedRecipe> saves, string searchString, ObjectId userId, ObjectId? authorId,
        List<Category>? filterList, CancellationToken cancellationToken)
    {
        return await this._recipesRepository.GetSavedRecipesAsync(pageNumber, pageSize, x => x.Name.Contains(searchString)
            || x.Categories.Any(c => c.Name.Contains(searchString)) || (x.Text != null && x.Text.Contains(searchString))
            || (x.IngredientsText != null && x.IngredientsText.Contains(searchString))
            || x.Categories.Any(c => filterList.Contains(c)) || x.CreatedById == authorId, saves, cancellationToken);

    }

    public async Task DeleteRecipeAsync(RecipeDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);
        entity.IsDeleted = true;
        await this._recipesRepository.UpdateRecipeAsync(entity, cancellationToken);
    }

    public async Task<PagedList<RecipeDto>> GetRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsDeleted == false, cancellationToken);
        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        var count = await this._recipesRepository.GetTotalCountAsync();
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
    }
}

