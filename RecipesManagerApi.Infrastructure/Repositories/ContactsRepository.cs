using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;
using MongoDB.Driver;


namespace RecipesManagerApi.Infrastructure.Repositories;

public class ContactsRepository : BaseRepository<Contact>, IContactsRepository
{    
    public ContactsRepository(MongoDbContext db) : base(db, "Contacts") {}    

    public async Task<Contact> GetContactAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateContactAsync(Contact contact, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<Contact>.Filter.Eq(x=>x.Id, contact.Id), contact, new ReplaceOptions(), cancellationToken);
    }
}
