using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class ContactsQuery
{
    public Task<PagedList<ContactDto>> GetContactsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, 
        [Service] IContactsService service)
        => service.GetContactsAsync(pageNumber, pageSize, cancellationToken);

    public Task<ContactDto> GetContactAsync(string id, CancellationToken cancellationToken, 
        [Service] IContactsService service)
        => service.GetContactAsync(id, cancellationToken);
}
