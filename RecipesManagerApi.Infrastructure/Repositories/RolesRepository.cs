using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Repositories;
public class RolesRepository : BaseRepository<Role>, IRolesRepository
{
    public RolesRepository(MongoDbContext db) : base(db, "Roles") { }
 
    public async Task<Role> GetRoleAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await(await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }
}
