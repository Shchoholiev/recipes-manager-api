using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories
{
	public class OpenAiLogsRepository : BaseRepository<OpenAiLog>,  IOpenAiLogsRepository
	{
		public OpenAiLogsRepository(MongoDbContext db) : base(db, "OpenAiLogs") { }

		public async Task<OpenAiLog> GetOpenAiLogAsync(ObjectId id, CancellationToken cancellationToken)
		{
			return await(await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<OpenAiLog> UpdateOpenAiLogAsync(OpenAiLog log, CancellationToken cancellationToken)
		{
			var updateDefinition = Builders<OpenAiLog>.Update
				.Set(l => l.Request, log.Request)
				.Set(l => l.Response, log.Response)
				.Set(l => l.LastModifiedById, log.LastModifiedById)
				.Set(l => l.LastModifiedDateUtc, log.LastModifiedDateUtc);
				
			var options = new FindOneAndUpdateOptions<OpenAiLog>
			{
				ReturnDocument = ReturnDocument.After
			};	
			
			return await this._collection.FindOneAndUpdateAsync(
				Builders<OpenAiLog>.Filter.Eq(l => l.Id, log.Id), updateDefinition, options, cancellationToken);
		}
	}
}
