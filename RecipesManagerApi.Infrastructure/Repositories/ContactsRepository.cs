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
		return await (await this._collection.FindAsync(x => x.Id == id && x.IsDeleted == false)).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<Contact> UpdateContactAsync(ObjectId id, Contact contact, CancellationToken cancellationToken)
	{
		var updateDefinition = Builders<Contact>.Update
			.Set(c => c.Name, contact.Name)
			.Set(c => c.Email, contact.Email)
			.Set(c => c.Phone, contact.Phone)
			.Set(c => c.LastModifiedById, contact.LastModifiedById)
			.Set(c => c.LastModifiedDateUtc, contact.LastModifiedDateUtc);
			
		var options = new FindOneAndUpdateOptions<Contact>()
		{
			ReturnDocument = ReturnDocument.After	
		};
		
		return await this._collection.FindOneAndUpdateAsync(
			Builders<Contact>.Filter.Eq(c => c.Id, id), updateDefinition, options, cancellationToken);
	}

	public new async Task<int> GetTotalCountAsync()
	{
		return (int)(await this._collection.CountDocumentsAsync<Contact>(x => x.IsDeleted == false));
	}
}

