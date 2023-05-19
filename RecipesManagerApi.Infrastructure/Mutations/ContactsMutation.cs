using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class ContactsMutation
{
     public Task<ContactDto> AddContactAsync(ContactCreateDto contact, CancellationToken cancellationToken, 
        [Service] IContactsService service)
        => service.AddContactAsync(contact, cancellationToken);

    public Task<ContactDto> UpdateContactAsync(string id, ContactCreateDto contact, CancellationToken cancellationToken,
        [Service] IContactsService service)
        => service.UpdateContactAsync(id, contact, cancellationToken);

    public Task DeleteContactAsync(string id, CancellationToken cancellationToken,
        [Service] IContactsService service)
        => service.DeleteContactAsync(id, cancellationToken);
    
}
