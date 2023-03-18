using MongoDB.Driver;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Domain.Common;
using MongoDB.Bson;
using System.Linq.Expressions;

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

        public async Task<ObjectId> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this._collection.InsertOneAsync(entity, cancellationToken);
            return entity.Id;
        }

        public async Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await this._collection.Find(Builders<TEntity>.Filter.Empty)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Limit(pageSize)
                                         .ToListAsync(cancellationToken); 
        }

        public async Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await this._collection.Find(predicate)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Limit(pageSize)
                                         .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return (int)(await this._collection.EstimatedDocumentCountAsync());
        }
    }
}