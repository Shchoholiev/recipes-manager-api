using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IOpenAiLogsService
{
    Task<OpenAiLogDto> AddLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken);

    Task<OpenAiLogDto> GetLogAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<OpenAiLogDto>> GetLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<PagedList<OpenAiLogDto>> GetLogsPageAsync(string usedId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<OpenAiLogDto> UpdateLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken);
}
