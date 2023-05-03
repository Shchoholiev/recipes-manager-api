using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class LogsService : ILogsService
{
    private readonly IMapper _mapper;

    private readonly ILogsRepository _repository;
    public LogsService(IMapper mapper, ILogsRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }
    public async Task<LogDto> AddLogAsync(LogDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Log>(dto);
        entity.CreatedDateUtc = DateTime.UtcNow;
        await this._repository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<LogDto>(entity);
    }

    public async Task<LogDto> GetLogAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetLogAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<Role>();
        }
        return this._mapper.Map<LogDto>(entity);
    }

    public async Task<PagedList<LogDto>> GetLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = this._mapper.Map<List<LogDto>>(entities);
        var count = await this._repository.GetTotalCountAsync();
        return new PagedList<LogDto>(dtos, pageNumber, pageSize, count);
    }
}
