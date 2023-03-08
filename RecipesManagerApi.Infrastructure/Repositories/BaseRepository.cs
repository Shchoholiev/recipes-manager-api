using MongoDB.Driver;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : EntityBase
    {
        protected MongoDbContext _db;

        protected IMongoCollection<TEntity> _collection;

        public BaseRepository(MongoDbContext db, string collectionName)
        {
            this._db = db;
            this._collection = _db.Db.GetCollection<TEntity>(collectionName);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this._collection.InsertOneAsync(entity, cancellationToken);
        }
    }
}