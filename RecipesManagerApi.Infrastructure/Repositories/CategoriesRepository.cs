using Amazon.Runtime.SharedInterfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;
using System.Collections;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    public class CategoriesRepository : BaseRepository<Category>,  ICategoriesRepository
    {
        public CategoriesRepository(MongoDbContext db) : base(db, "Categories") { }

        public async Task<Category> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken)
        {
            var filter = Builders<Category>.Filter.Eq("_id", id);
            return await (await this._collection.FindAsync(filter)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Category>> GetPageCategoriesAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            return await this._collection.Find(new BsonDocument())
                                         .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                         .Limit(pageParameters.PageSize)
                                         .ToListAsync(cancellationToken); 
        }

        public async Task<List<Category>> GetPageCategoriesAsync(PageParameters pageParameters, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            return await this._collection.Find(predicate)
                                         .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                         .Limit(pageParameters.PageSize)
                                         .ToListAsync(cancellationToken);
        }

        public async Task<long> GetTotalCounAsync()
        {
            return await this._collection.EstimatedDocumentCountAsync();
        }
    }
}
