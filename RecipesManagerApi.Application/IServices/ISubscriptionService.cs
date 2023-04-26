using System;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface ISubscriptionService
{
    Task<SubscriptionDto> AddSubscriptionAsync(SubscriptionCreateDto dto, CancellationToken cancellationToken);

    Task<SubscriptionDto> GetSubscriptionAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<SubscriptionDto>> GetSubscriptionsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<PagedList<SubscriptionDto>> GetSubscriptionsPageAsync(int pageNumber, int pageSize, string userId, CancellationToken cancellationToken);

    Task<PagedList<SubscriptionDto>> GetOwnSubscriptionsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<SubscriptionDto> UpdateSubscriptionAsync(SubscriptionDto dto, CancellationToken cancellationToken);

    Task<OperationDetails> DeleteSubscriptionAsync(SubscriptionDto dto, CancellationToken cancellationToken);
}

