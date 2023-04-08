using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IContactsService
{
    Task AddContactAsync(ContactDto dto, CancellationToken cancellationToken);

    Task UpdateContactAsync(ContactDto dto, CancellationToken cancellationToken);

    Task DeleteContactAsync(ContactDto dto, CancellationToken cancellationToken);

    Task<ContactDto> GetContactAsync(ObjectId id, CancellationToken cancellationToken);

    Task<PagedList<ContactDto>> GetContactsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
