using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    public class OpenAiLogsRepository : BaseRepository<OpenAiLog>,  IOpenAiLogsRepository
    {
        public OpenAiLogsRepository(MongoDbContext db) : base(db, "OpenAiLogs") { }

        public async Task UpdateAsync(OpenAiLog log, CancellationToken cancellationToken)
        {
            await this._collection.ReplaceOneAsync(Builders<OpenAiLog>.Filter.Eq(l => l.Id, log.Id), log, new ReplaceOptions(), cancellationToken);
        }
    }
}
