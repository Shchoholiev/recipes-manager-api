using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class LogsQuery
{
    [Authorize]
    public Task<PagedList<LogDto>> GetLogsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ILogsService service)
        => service.GetLogsPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<LogDto> GetLogAsync(string id, CancellationToken cancellationToken,
        [Service] ILogsService service)
        => service.GetLogAsync(id, cancellationToken);
}
