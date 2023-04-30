using MongoDB.Bson;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IShoppingListsRepository : IBaseRepository<ShoppingList>
{
	Task<ShoppingListLookedUp> GetShoppingListAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task UpdateShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken);
	
	Task<ShoppingListLookedUp> AddShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken);
}