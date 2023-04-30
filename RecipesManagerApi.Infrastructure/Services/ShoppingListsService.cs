using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class ShoppingListsService : IShoppingListsService
{
	private readonly IMapper _mapper;
	
	private readonly IShoppingListsRepository _shoppingListsRepository;
	
	public ShoppingListsService(IMapper mapper, IShoppingListsRepository shoppingListsRepository)
	{
		this._mapper = mapper;
		this._shoppingListsRepository = shoppingListsRepository;
	}

	public async Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<ShoppingList>(shoppingList);
		var newEntity = await this._shoppingListsRepository.AddShoppingListAsync(entity, cancellationToken);
		
		return this._mapper.Map<ShoppingListDto>(newEntity);
	}

	public async Task<OperationDetails> DeleteShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken)
	{
		List<string> recipesIds = new List<string>();
		if(shoppingList.Recipes != null)
		{
			recipesIds = shoppingList.Recipes.Select(x => x.Id).ToList();
		}
		var entity = this._mapper.Map<ShoppingList>(shoppingList, opt => opt.Items["RecipesIds"] =  recipesIds);
		entity.IsDeleted = true;
		await this._shoppingListsRepository.UpdateShoppingListAsync(entity, cancellationToken);
		return new OperationDetails() { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}

	public async Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken)
	{
		ObjectId.TryParse(id, out ObjectId objectId);
		var entity = await this._shoppingListsRepository.GetShoppingListAsync(objectId, cancellationToken);
		if(entity == null)
		{
			throw new EntityNotFoundException<ShoppingList>();
		}
		return this._mapper.Map<ShoppingListDto>(entity);
	}

	public async Task<ShoppingListDto> UpdateShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken)
	{
		List<string> recipesIds = new List<string>();
		if(shoppingList.Recipes != null)
		{
			recipesIds = shoppingList.Recipes.Select(x => x.Id).ToList();
		}
		var entity = this._mapper.Map<ShoppingList>(shoppingList, opt => opt.Items["RecipesIds"] =  recipesIds);
		await this._shoppingListsRepository.UpdateShoppingListAsync(entity, cancellationToken);
		
		return shoppingList;
	}
	
    public Task<OperationDetails> SendShoppingListToEmailAsync(string id, IEnumerable<string> emailsTo, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}