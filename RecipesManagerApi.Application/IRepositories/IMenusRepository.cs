using System.Linq.Expressions;
using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IMenusRepository : IBaseRepository<Menu>
{
	Task<MenuLookedUp> GetMenuLookedUpAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task<Menu> GetMenuAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task<List<MenuLookedUp>> GetPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken);
	
	Task<MenuLookedUp> UpdateMenuAsync(ObjectId id, Menu menu, CancellationToken cancellationToken);
	
	Task UpdateMenuSentToAsync(Menu menu, CancellationToken cancellationToken);
	
	Task<int> GetTotalCountAsync(Expression<Func<Menu, bool>> predicate);
	
	Task<MenuLookedUp> AddMenuAsync(Menu entity, CancellationToken cancellationToken);
}
