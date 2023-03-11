using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;
public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;

    private readonly IMapper _mapper;

    public UsersService(IUsersRepository repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task AddUserAsync(UserDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<User>(dto);
        await this._repository.AddAsync(entity, cancellationToken);
    }

    public async Task<PagedList<UserDto>> GetUsersPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetUserPageAsync(pageParameters, cancellationToken);
        var dtos = this._mapper.Map<List<UserDto>>(entities);
        var count = await this._repository.GetTotalCountAsync();
        return new PagedList<UserDto>(dtos, pageParameters, count);
    }

    public async Task<UserDto> GetUserAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var entity = await this._repository.GetUserAsync(id, cancellationToken);
        return this._mapper.Map<UserDto>(entity);
    }

    public async Task UpdateUserAsync(UserDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<User>(dto);
        await this._repository.UpdateUserAsync(entity, cancellationToken);
    }
}
