using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IOpenAiLogsService
{
    Task<OpenAiLogDto> AddOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken);

    Task<OpenAiLogDto> GetOpenAiLogAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<OpenAiLogDto>> GetOpenAiLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<OpenAiLogDto> UpdateOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken);
}
