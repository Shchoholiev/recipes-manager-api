using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class OpenAiLogsMutation
{
    [Authorize]
    public Task<OpenAiLogDto> UpdateOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken,
        [Service] IOpenAiLogsService service)
        => service.UpdateOpenAiLogAsync(dto, cancellationToken);

    [Authorize]
    public Task<OpenAiLogDto> AddOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken,
    [Service] IOpenAiLogsService service)
    => service.AddOpenAiLogAsync(dto, cancellationToken);
}
