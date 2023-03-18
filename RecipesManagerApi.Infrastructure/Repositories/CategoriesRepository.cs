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
            return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
