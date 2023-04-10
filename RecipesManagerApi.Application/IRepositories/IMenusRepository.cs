using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IMenusRepository : IBaseRepository<Menu>
{
	Task<Menu> GetMenuAsync(ObjectId id, CancellationToken cancellationToken);
	Task UpdateMenuAsync(Menu menu, CancellationToken cancellationToken);
}
