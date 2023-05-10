using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
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

    public async Task<PagedList<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = this._mapper.Map<List<UserDto>>(entities);
        var count = await this._repository.GetTotalCountAsync();
        return new PagedList<UserDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task<UserDto> GetUserAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId)) {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetUserAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<User>();
        }

        return this._mapper.Map<UserDto>(entity);
    }

    public async Task UpdateUserAsync(UserDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<User>(dto);
        await this._repository.UpdateUserAsync(entity, cancellationToken);
    }
}
