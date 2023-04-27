using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class SubscriptionsMutation
{
    [Authorize]
    public Task<SubscriptionDto> AddSubscriptionAsync(SubscriptionCreateDto dto, CancellationToken cancellationToken,
     [Service] ISubscriptionService service)
     => service.AddSubscriptionAsync(dto, cancellationToken);

    [Authorize]
    public Task<SubscriptionDto> UpdateSubscriptionAsync(SubscriptionDto dto, CancellationToken cancellationToken,
    [Service] ISubscriptionService service)
    => service.UpdateSubscriptionAsync(dto, cancellationToken);

    [Authorize]
    public Task<OperationDetails> DeleteSubscriptionAsync(string id, CancellationToken cancellationToken,
    [Service] ISubscriptionService service)
    => service.DeleteSubscriptionAsync(id, cancellationToken);
}

