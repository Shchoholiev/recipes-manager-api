﻿using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Repositories;
public class UsersRepository : BaseRepository<User>, IUsersRepository
{
	public UsersRepository(MongoDbContext db) : base(db, "Users") { }

	public async Task<User> GetUserAsync(ObjectId id, CancellationToken cancellationToken)
	{
		return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<User> GetUserAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
	{
		return await(await this._collection.FindAsync(predicate)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<User>.Update
			.Set(u => u.Name, user.Name)
			.Set(u => u.Email, user.Email)
			.Set(u => u.Phone, user.Phone)
			.Set(u => u.RefreshToken, user.RefreshToken)
			.Set(u => u.RefreshTokenExpiryDate, user.RefreshTokenExpiryDate)
			.Set(u => u.AppleDeviceId, user.AppleDeviceId)
			.Set(u => u.WebId, user.WebId)
			.Set(u => u.Roles, user.Roles);
		
		var options = new FindOneAndUpdateOptions<User>
		{
			ReturnDocument = ReturnDocument.After
		};
		
		return await this._collection.FindOneAndUpdateAsync(
			Builders<User>.Filter.Eq(u => u.Id, user.Id), updateDefinition, options, cancellationToken);
			
	}

}
