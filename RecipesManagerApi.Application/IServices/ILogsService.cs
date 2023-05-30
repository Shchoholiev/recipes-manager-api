using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface ILogsService
{
    Task<LogDto> AddLogAsync(LogDto dto, CancellationToken cancellationToken);

    Task<LogDto> GetLogAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<LogDto>> GetLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
