using MongoDB.Bson;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IContactsService
{
    Task<ContactDto> AddContactAsync(ContactCreateDto dto, CancellationToken cancellationToken);

    Task<ContactDto> UpdateContactAsync(string id, ContactCreateDto dto, CancellationToken cancellationToken);

    Task<OperationDetails> DeleteContactAsync(string id, CancellationToken cancellationToken);

    Task<ContactDto> GetContactAsync(string id, CancellationToken cancellationToken);

    Task<PagedList<ContactDto>> GetContactsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
