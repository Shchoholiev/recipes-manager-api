using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class OpenAiLogsQuery
{
    [Authorize(Roles = new[] {"Admin"})]
    public Task<PagedList<OpenAiLogDto>> GetOpenAiLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.GetLogsPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize(Roles = new[] { "Admin" })]
    public Task<PagedList<OpenAiLogDto>> GetOpenAiLogsPageByUserIdAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.GetLogsPageAsync(userId, pageNumber, pageSize, cancellationToken);

    [Authorize(Roles = new[] { "Admin" })]
    public Task<OpenAiLogDto> GetOpenAiLogAsync(string id, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.GetLogAsync(id, cancellationToken);
}
