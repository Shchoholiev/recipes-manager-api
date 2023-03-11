﻿using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;
public interface IUsersService
{
    Task AddUserAsync(UserDto dto, CancellationToken cancellationToken);

    Task<PagedList<UserDto>> GetPageUsersAsync(PageParameters pageParameters, CancellationToken cancellationToken);

    Task<UserDto> GetUserAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateUserAsync (UserDto dto, CancellationToken cancellationToken);
}
