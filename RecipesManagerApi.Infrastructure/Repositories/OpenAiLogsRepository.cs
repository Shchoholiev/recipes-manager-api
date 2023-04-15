using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    public class OpenAiLogsRepository : BaseRepository<OpenAiLog>,  IOpenAiLogsRepository
    {
        public OpenAiLogsRepository(MongoDbContext db) : base(db, "OpenAiLogs") { }
    }
}
