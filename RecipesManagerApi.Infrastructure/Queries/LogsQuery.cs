using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class LogsQuery
{
    public Task<PagedList<LogDto>> GetLogsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ILogsService service)
        => service.GetLogsPageAsync(pageNumber, pageSize, cancellationToken);

    public Task<LogDto> GetLogAsync(string id, CancellationToken cancellationToken,
        [Service] ILogsService service)
        => service.GetLogAsync(id, cancellationToken);
}
