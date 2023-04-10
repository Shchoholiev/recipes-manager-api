using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class MenusRepository : BaseRepository<Menu>, IMenusRepository
{
	public MenusRepository(MongoDbContext db) : base (db, "Menus"){ }
	public async Task<Menu> GetMenuAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task UpdateMenuAsync(Menu menu, CancellationToken cancellationToken)
	{
		await this._collection.ReplaceOneAsync(x => x.Id == menu.Id, menu, new ReplaceOptions(), cancellationToken);
	}
}
