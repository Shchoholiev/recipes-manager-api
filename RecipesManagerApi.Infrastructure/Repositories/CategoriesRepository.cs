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
    public class CategoriesRepository : ICategoriesRepository
    {
        private MongoDbContext _db;

        private IMongoCollection<Category> _collection;

        public CategoriesRepository(MongoDbContext db)
        {
            this._db = db;
            this._collection = _db.Db.GetCollection<Category>("Categories");
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            var filter = Builders<Category>.Filter.Eq("_id", id);
            var bson =  await this._collection.FindAsync(filter);
            return BsonSerializer.Deserialize<Category>((BsonDocument)bson);
        }

        public Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var bson = await this._collection.FindAsync(filter);
            return BsonSerializer.Deserialize<Category>((BsonDocument)bson);
        }

        public Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
