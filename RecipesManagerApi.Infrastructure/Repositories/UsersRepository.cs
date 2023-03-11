using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Repositories;
public class UsersRepository : BaseRepository<User>, IUsersRepository
{
    public UsersRepository(MongoDbContext db) : base(db, "Users") { } 

    public async Task<User> GetUserAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<User>> GetUserPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
    {
        return await this._collection.Find(Builders<User>.Filter.Empty)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Limit(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetUserPageAsync(PageParameters pageParameters, Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        return await this._collection.Find(predicate)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Limit(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(x => x.Id == user.Id, user, new ReplaceOptions(), cancellationToken);
    }
}
