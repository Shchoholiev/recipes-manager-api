using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface ILogsRepository : IBaseRepository<Log>
{
    Task<Log> GetLogAsync(ObjectId id, CancellationToken cancellationToken);
}
