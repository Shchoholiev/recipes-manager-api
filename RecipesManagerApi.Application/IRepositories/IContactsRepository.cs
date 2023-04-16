using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories;

public interface IContactsRepository : IBaseRepository<Contact>
{
    Task<Contact> GetContactAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken);
}
