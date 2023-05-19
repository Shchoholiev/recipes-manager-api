using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IOpenAiLogsRepository : IBaseRepository<OpenAiLog>
{
    Task<OpenAiLog> UpdateOpenAiLogAsync(OpenAiLog log, CancellationToken cancellationToken);

    Task<OpenAiLog> GetOpenAiLogAsync(ObjectId id, CancellationToken cancellationToken);
}
