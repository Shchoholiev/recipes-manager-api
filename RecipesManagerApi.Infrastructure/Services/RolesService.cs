using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;
public class RolesService : IRolesService
{
    private readonly IRolesRepository _repository;

    private readonly IMapper _mapper;

    public RolesService(IRolesRepository repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task AddRoleAsync(RoleDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Role>(dto);
        await this._repository.AddAsync(entity, cancellationToken);
    }

    public async Task<PagedList<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = this._mapper.Map<List<RoleDto>>(entities);
        var count = await this._repository.GetTotalCountAsync();
        return new PagedList<RoleDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task<RoleDto> GetRoleAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId)) {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetRoleAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<Role>();
        }
        return this._mapper.Map<RoleDto>(entity);
    }
}
