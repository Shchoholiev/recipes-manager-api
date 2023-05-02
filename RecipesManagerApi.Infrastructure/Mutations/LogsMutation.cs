using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class LogsMutation
{
    public Task<LogDto> AddLogAsync(LogDto dto, CancellationToken cancellationToken,
        [Service] ILogsService userManager)
        => userManager.AddLogAsync(dto, cancellationToken);
}
