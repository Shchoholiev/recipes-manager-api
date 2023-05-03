using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class OpenAiLogsService : IOpenAiLogsService
{
    private readonly IOpenAiLogsRepository _repository;

    private readonly IMapper _mapper;

    public OpenAiLogsService(IOpenAiLogsRepository repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    public async Task<OpenAiLogDto> AddOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<OpenAiLog>(dto);
        entity.CreatedDateUtc = DateTime.UtcNow;
        await this._repository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<OpenAiLogDto>(entity);
    }

    public async Task<OpenAiLogDto> GetOpenAiLogAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var entity = await this._repository.GetOpenAiLogAsync(objectId, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException<Role>();
        }
        return this._mapper.Map<OpenAiLogDto>(entity);
    }

    public async Task<PagedList<OpenAiLogDto>> GetOpenAiLogsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = this._mapper.Map<List<OpenAiLogDto>>(entities);
        var count = await this._repository.GetTotalCountAsync();
        return new PagedList<OpenAiLogDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task<OpenAiLogDto> UpdateOpenAiLogAsync(OpenAiLogDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<OpenAiLog>(dto);
        await this._repository.UpdateOpenAiLogAsync(entity, cancellationToken);
        return this._mapper.Map<OpenAiLogDto>(entity);
    }
}
