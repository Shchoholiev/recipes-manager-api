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
using RecipesManagerApi.Application.Exceptions;

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

    public async Task<RecipeDto> AddRecipeAsync(RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Recipe>(dto);

        entity.CreatedById = GlobalUser.Id.Value;
        entity.CreatedDateUtc = DateTime.UtcNow;

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

        return this._mapper.Map<RecipeDto>(recipe);
    }

    public async Task<RecipeDto> UpdateRecipeAsync(string id, RecipeCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var recipeId))
        {
            throw new EntityNotFoundException<Recipe>();
        }

        var entity = this._mapper.Map<Recipe>(dto);
        entity.LastModifiedById = GlobalUser.Id.Value;
        entity.LastModifiedDateUtc = DateTime.UtcNow;

        var updated = await this._recipesRepository.UpdateRecipeAsync(recipeId, entity, cancellationToken);
        if (dto.Thumbnail != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await dto.Thumbnail.CopyToAsync(memoryStream, cancellationToken);
                var extension = System.IO.Path.GetExtension(dto.Thumbnail.FileName).Substring(1).ToLower();
                Task.Run(() => _imagesService.AddRecipeImageAsync(memoryStream.ToArray(), extension, updated.Id, cancellationToken));
            }
        }

        return this._mapper.Map<RecipeDto>(updated);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var recipeId))
        {
            throw new EntityNotFoundException<Recipe>();
        }

        var recipe = new Recipe
        {
            Id = recipeId,
            LastModifiedById = GlobalUser.Id.Value,
            LastModifiedDateUtc = DateTime.UtcNow
        };

        await this._recipesRepository.DeleteAsync(recipe, cancellationToken);
    }

    public async Task<PagedList<RecipeDto>> GetSearchPageAsync(int pageNumber, int pageSize, string searchString, string? authorsId,
        List<string>? categoriesIds, RecipesSearchTypes recipeSearchType, CancellationToken cancellationToken)
    {
        var categoriesFilter = new List<ObjectId>();
        foreach (var c in categoriesIds ?? Enumerable.Empty<string>())
        {
            if (ObjectId.TryParse(c, out var id))
            {
                categoriesFilter.Add(id);
            }
        }

        ObjectId.TryParse(authorsId, out var authorId);
        var userId = GlobalUser.Id.Value;

        switch (recipeSearchType)
        {
            case RecipesSearchTypes.Personal:
                return await this.GetPersonalRecipesPageAsync(pageNumber, pageSize, searchString, userId, categoriesFilter, cancellationToken);

            case RecipesSearchTypes.Public:
                return await this.GetPublicRecipesPageAsync(pageNumber, pageSize, searchString, userId, authorId, categoriesFilter, cancellationToken);

            case RecipesSearchTypes.Subscribed:
                return await this.GetSubscribedRecipesPageAsync(pageNumber, pageSize, searchString, userId, authorId, categoriesFilter, cancellationToken);

            case RecipesSearchTypes.Saved:
                return await this.GetSavedRecipesPageAsync(pageNumber, pageSize, searchString, userId, authorId, categoriesFilter, cancellationToken);

            default:
                throw new NotImplementedException();
        }
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

    private async Task<PagedList<RecipeDto>> GetPublicRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, ObjectId authorId,
        List<ObjectId> categoriesIds, CancellationToken cancellationToken)
    {
        Expression<Func<Recipe, bool>> predicate = (Recipe r)
            => !r.IsDeleted && r.IsPublic && r.CreatedById != userId
            && (r.Name.Contains(searchString)
                || (!string.IsNullOrEmpty(r.Text) && r.Text.Contains(searchString))
                || (!string.IsNullOrEmpty(r.IngredientsText) && r.IngredientsText.Contains(searchString))
                || (r.Ingredients != null && r.Ingredients.Any(i => i.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => c.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => categoriesIds.Contains(c.Id)))
                || (authorId != ObjectId.Empty && r.CreatedById == authorId)
            );

        var recipesTask = this._recipesRepository.GetRecipesPageAsync(pageNumber, pageSize, predicate, cancellationToken);
        var countTask = this._recipesRepository.GetTotalCountAsync(predicate, cancellationToken);
        await Task.WhenAll(recipesTask, countTask);

        var entities = recipesTask.Result;
        var count = countTask.Result;

        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
    }

    private async Task<PagedList<RecipeDto>> GetPersonalRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId,
        List<ObjectId> categoriesIds, CancellationToken cancellationToken)
    {
        Expression<Func<Recipe, bool>> predicate = (Recipe r)
            => !r.IsDeleted && r.CreatedById == userId
            && (r.Name.Contains(searchString)
                || (!string.IsNullOrEmpty(r.Text) && r.Text.Contains(searchString))
                || (!string.IsNullOrEmpty(r.IngredientsText) && r.IngredientsText.Contains(searchString))
                || (r.Ingredients != null && r.Ingredients.Any(i => i.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => c.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => categoriesIds.Contains(c.Id)))
            );

        var recipesTask = this._recipesRepository.GetRecipesPageAsync(pageNumber, pageSize, predicate, cancellationToken);
        var countTask = this._recipesRepository.GetTotalCountAsync(predicate, cancellationToken);
        await Task.WhenAll(recipesTask, countTask);

        var entities = recipesTask.Result;
        var count = countTask.Result;

        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
    }

    private async Task<PagedList<RecipeDto>> GetSubscribedRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, ObjectId authorId,
        List<ObjectId> categoriesIds, CancellationToken cancellationToken)
    {
        Expression<Func<Recipe, bool>> predicate = (Recipe r)
            => !r.IsDeleted
            && (r.Name.Contains(searchString)
                || (!string.IsNullOrEmpty(r.Text) && r.Text.Contains(searchString))
                || (!string.IsNullOrEmpty(r.IngredientsText) && r.IngredientsText.Contains(searchString))
                || (r.Ingredients != null && r.Ingredients.Any(i => i.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => c.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => categoriesIds.Contains(c.Id)))
                || (authorId != ObjectId.Empty && r.CreatedById == authorId)
            );

        var recipesTask = this._recipesRepository.GetSubscribedRecipesAsync(pageNumber, pageSize, userId, predicate, cancellationToken);
        var countTask = this._recipesRepository.GetSubscriptionsCountAsync(predicate, userId, cancellationToken);
        await Task.WhenAll(recipesTask, countTask);

        var entities = recipesTask.Result;
        var count = countTask.Result;

        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
    }


    private async Task<PagedList<RecipeDto>> GetSavedRecipesPageAsync(int pageNumber, int pageSize, string searchString, ObjectId userId, ObjectId? authorId,
        List<ObjectId> categoriesIds, CancellationToken cancellationToken)
    {
        Expression<Func<Recipe, bool>> predicate = (Recipe r)
            => !r.IsDeleted
            && (r.Name.Contains(searchString)
                || (!string.IsNullOrEmpty(r.Text) && r.Text.Contains(searchString))
                || (!string.IsNullOrEmpty(r.IngredientsText) && r.IngredientsText.Contains(searchString))
                || (r.Ingredients != null && r.Ingredients.Any(i => i.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => c.Name.Contains(searchString)))
                || (r.Categories != null && r.Categories.Any(c => categoriesIds.Contains(c.Id)))
            );

        var recipesTask = this._recipesRepository.GetSavedRecipesAsync(pageNumber, pageSize, userId, predicate, cancellationToken);
        var countTask = this._recipesRepository.GetSavedRecipesCountAsync(predicate, userId, cancellationToken);
        await Task.WhenAll(recipesTask, countTask);

        var entities = recipesTask.Result;
        var count = countTask.Result;

        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);

    }

    public async Task<PagedList<RecipeDto>> GetRecipesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._recipesRepository.GetPageAsync(pageNumber, pageSize, x => x.IsDeleted == false, cancellationToken);
        var dtos = this._mapper.Map<List<RecipeDto>>(entities);
        var count = await this._recipesRepository.GetTotalCountAsync();
        return new PagedList<RecipeDto>(dtos, pageNumber, pageSize, count);
    }
}

