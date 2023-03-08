using MongoDB.Driver;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    abstract class BaseRepository<T> where T : EntityBase
    {
        private MongoDbContext _db;

        private IMongoCollection<T> _collection;

        public BaseRepository(MongoDbContext db, string collectionName)
        {
            this._db = db;
            this._collection = _db.Db.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity)
        {
            await this._collection.InsertOneAsync(entity);
        }
    }
}