﻿using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Infrastructure.Services;

public class SubscriptionsService : ISubscriptionService
{
	private readonly IMapper _mapper;

	private readonly ISubscriptionsRepository _repository;

	public SubscriptionsService(IMapper mapper, ISubscriptionsRepository repository)
	{
		this._mapper = mapper;
		this._repository = repository;
	}

	public async Task<SubscriptionDto> AddSubscriptionAsync(SubscriptionCreateDto dto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Subscription>(dto);
		entity.CreatedById = GlobalUser.Id.Value;
		entity.CreatedDateUtc = DateTime.UtcNow;
		await this._repository.AddAsync(entity, cancellationToken);
		return this._mapper.Map<SubscriptionDto>(entity);
	}

	public async Task<OperationDetails> DeleteSubscriptionAsync(string id, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(id, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		
		var subscription = new Subscription
		{
			Id = objectId,
			LastModifiedById = GlobalUser.Id.Value,
			LastModifiedDateUtc = DateTime.UtcNow
		};
		
		await this._repository.DeleteAsync(subscription, cancellationToken);
		return new OperationDetails() { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}

	public async Task<SubscriptionDto> GetSubscriptionAsync(string id, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(id, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}

		var entity = await this._repository.GetSubscriptionAsync(objectId, cancellationToken);
		if (entity == null || entity.IsDeleted == true)
		{
			throw new EntityNotFoundException<Subscription>();
		}
		return this._mapper.Map<SubscriptionDto>(entity);
	}

	public async Task<PagedList<SubscriptionDto>> GetSubscriptionsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		var userId = GlobalUser.Id.Value;
		var entities = await this._repository.GetPageAsync(pageNumber, pageSize, x => x.AuthorId == userId && x.IsDeleted == false, cancellationToken);
		var dtos = this._mapper.Map<List<SubscriptionDto>>(entities);
		var count = await this._repository.GetTotalCountAsync(x => x.IsDeleted == false);
		return new PagedList<SubscriptionDto>(dtos, pageNumber, pageSize, count);
	}

	public async Task<PagedList<SubscriptionDto>> GetOwnSubscriptionsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		var userId = GlobalUser.Id.Value;
		var entities = await this._repository.GetPageAsync(pageNumber, pageSize, x => x.CreatedById == userId && x.IsDeleted == false, cancellationToken);
		var dtos = this._mapper.Map<List<SubscriptionDto>>(entities);
		var count = await this._repository.GetTotalCountAsync(x => x.IsDeleted == false);
		return new PagedList<SubscriptionDto>(dtos, pageNumber, pageSize, count);
	}

	public async Task<PagedList<SubscriptionDto>> GetSubscriptionsPageAsync(int pageNumber, int pageSize, string userId, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(userId, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var entities = await this._repository.GetPageAsync(pageNumber, pageSize, x => x.CreatedById == objectId && x.IsDeleted == false, cancellationToken);
		var dtos = this._mapper.Map<List<SubscriptionDto>>(entities);
		var count = await this._repository.GetTotalCountAsync(x => x.IsDeleted == false);
		return new PagedList<SubscriptionDto>(dtos, pageNumber, pageSize, count);
	}

	public async Task<SubscriptionDto> UpdateSubscriptionAsync(SubscriptionDto dto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Subscription>(dto);
		if (entity.AuthorId != GlobalUser.Id.Value) {
			throw new UnauthorizedEntityUpdateException<Subscription>();
		}
		entity.LastModifiedById = GlobalUser.Id.Value;
		entity.LastModifiedDateUtc = DateTime.UtcNow;
		var updated = await this._repository.UpdateSubscriptionAsync(entity, cancellationToken);
		return this._mapper.Map<SubscriptionDto>(updated);
	}
}

