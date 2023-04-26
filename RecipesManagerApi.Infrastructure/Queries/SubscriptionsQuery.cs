using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using HotChocolate.Authorization;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class SubscriptionsQuery
{
    [Authorize]
    public Task<PagedList<SubscriptionDto>> GetYourSubscriptionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ISubscriptionService service)
        => service.GetSubscriptionsPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<PagedList<SubscriptionDto>> GetOwnSubscriptionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
       [Service] ISubscriptionService service)
       => service.GetOwnSubscriptionsPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<PagedList<SubscriptionDto>> GetAuthorsSubscriptionsAsync(int pageNumber, int pageSize, string userId, CancellationToken cancellationToken,
        [Service] ISubscriptionService service)
        => service.GetSubscriptionsPageAsync(pageNumber, pageSize, userId, cancellationToken);
}

