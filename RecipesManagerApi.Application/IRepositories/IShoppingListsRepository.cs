using System.Linq.Expressions;
using MongoDB.Bson;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IShoppingListsRepository : IBaseRepository<ShoppingList>
{
	Task<ShoppingListLookedUp> GetShoppingListLookedUpAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task<ShoppingList> GetShoppingListAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task<ShoppingListLookedUp> UpdateShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken);
	
	Task<ShoppingListLookedUp> AddShoppingListAsync(ShoppingList shoppingList, CancellationToken cancellationToken);
	
	Task<List<ShoppingListLookedUp>> GetPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken);
	
	Task<int> GetTotalCountAsync(Expression<Func<ShoppingList, bool>> predicate);
}