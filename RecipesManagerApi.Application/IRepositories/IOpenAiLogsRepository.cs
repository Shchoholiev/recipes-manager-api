using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IOpenAiLogsRepository : IBaseRepository<OpenAiLog>
{
    Task UpdateAsync(OpenAiLog log, CancellationToken cancellationToken);
}
