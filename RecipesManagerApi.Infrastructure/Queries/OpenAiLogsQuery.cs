using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class OpenAiLogsQuery
{
    [Authorize]
    public Task<PagedList<OpenAiLogDto>> GetOpenAiLogsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.GetOpenAiLogsPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<OpenAiLogDto> GetOpenAiLogAsync(string id, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.GetOpenAiLogAsync(id, cancellationToken);
}
