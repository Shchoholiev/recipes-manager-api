using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class LogsRepository : BaseRepository<Log>, ILogsRepository
{
    public LogsRepository(MongoDbContext db) : base(db, "Logs") { }

    public async Task<Log> GetLogAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await(await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }
}
