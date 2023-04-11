using MongoDB.Bson;
using AutoMapper;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Infrastructure.Services;

public class ContactsService : IContactsService
{

    private readonly IContactsRepository _contactsRepository;
    private readonly IMapper _mapper;

    public ContactsService(IContactsRepository contactsRepository, IMapper mapper)
    {
        this._contactsRepository = contactsRepository;
        this._mapper = mapper;
    }

    public async Task<ContactDto> AddContactAsync(ContactCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        var newEntity = await this._contactsRepository.AddAsync(entity, cancellationToken);
        return this._mapper.Map<ContactDto>(newEntity);
    }

    public async Task<ContactDto> DeleteContactAsync(ContactDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        entity.IsDeleted = true;
        await this._contactsRepository.UpdateContactAsync(entity, cancellationToken);
        return dto;
    }

    public async Task<ContactDto> GetContactAsync(string id, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId)) {
                throw new InvalidDataException("Provided id is invalid.");
        }
        var entity = await this._contactsRepository.GetContactAsync(objectId, cancellationToken);
        if (entity == null)
            {
                throw new EntityNotFoundException<Contact>();
            }
        if (entity.IsDeleted)
            {
                throw new EntityIsDeletedException<Contact>();
            }
        return this._mapper.Map<ContactDto>(entity);
    }

    public async Task<PagedList<ContactDto>> GetContactsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities =  await this._contactsRepository.GetPageAsync(pageNumber, pageSize,x => x.IsDeleted == false ,cancellationToken);
        var dtos = this._mapper.Map<List<ContactDto>>(entities);
        var count = await this._contactsRepository.GetTotalCountAsync();
        return new PagedList<ContactDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task<ContactDto> UpdateContactAsync(ContactDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        ObjectId.TryParse(dto.Id, out var objectId);
        if ((await this._contactsRepository.GetContactAsync(objectId, cancellationToken)).IsDeleted)
            {
                throw new EntityIsDeletedException<Contact>();
            }
        await this._contactsRepository.UpdateContactAsync(entity, cancellationToken);
        return this._mapper.Map<ContactDto>(entity);
    }
}
