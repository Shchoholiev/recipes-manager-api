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
        private MongoDbContext _db;

        private IMongoCollection<Category> _collection;

        public CategoriesRepository(MongoDbContext db, string collectionName) : base(db, collectionName)
        {
            this._db = db;
            this._collection = _db.Db.GetCollection<Category>("Categories");
        }

        public async Task<Category> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken)
        {
            var filter = Builders<Category>.Filter.Eq("_id", id);
            return await (await this._collection.FindAsync(filter)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var categories = await this._collection.Find(x => true)
                                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                   .Limit(pageParameters.PageSize)
                                                   .ToListAsync(cancellationToken); 

            var page =  new PagedList<Category>(categories, pageParameters, categories.Count());
            return page;
        }

        public async Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            var categories = await this._collection.Find(predicate)
                                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                   .Limit(pageParameters.PageSize)
                                                   .ToListAsync(cancellationToken);

            var page = new PagedList<Category>(categories, pageParameters, categories.Count());
            return page;
        }
    }
}
