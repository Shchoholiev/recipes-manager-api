using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IContactsService
{
    Task<ContactDto> AddContactAsync(ContactCreateDto dto, CancellationToken cancellationToken);

    Task<ContactDto> UpdateContactAsync(ContactDto dto, CancellationToken cancellationToken);

    Task DeleteContactAsync(ContactDto dto, CancellationToken cancellationToken);

    Task<ContactDto> GetContactAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<ContactDto>> GetContactsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
