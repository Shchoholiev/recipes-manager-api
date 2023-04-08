using MongoDB.Bson;
using AutoMapper;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

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

    public async Task AddContactAsync(ContactDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        await this._contactsRepository.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteContactAsync(ContactDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        entity.IsDeleted = true;
        await this._contactsRepository.UpdateContactAsync(entity, cancellationToken);
    }

    public async Task<ContactDto> GetContactAsync(ObjectId id, CancellationToken cancellationToken)
    {
        var entity = await this._contactsRepository.GetContactAsync(id, cancellationToken);
        return this._mapper.Map<ContactDto>(entity);
    }

    public async Task<PagedList<ContactDto>> GetContactsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var entities =  await this._contactsRepository.GetPageAsync(pageNumber, pageSize, cancellationToken);
        var dtos = this._mapper.Map<List<ContactDto>>(entities);
        var count = await this._contactsRepository.GetTotalCountAsync();
        return new PagedList<ContactDto>(dtos, pageNumber, pageSize, count);
    }

    public async Task UpdateContactAsync(ContactDto dto, CancellationToken cancellationToken)
    {
        var entity = this._mapper.Map<Contact>(dto);
        await this._contactsRepository.UpdateContactAsync(entity, cancellationToken);
    }
}
