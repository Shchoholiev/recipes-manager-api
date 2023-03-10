using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class RecipesRepository : BaseRepository<Recipe>, IRecipesRepository
{
    public RecipesRepository(MongoDbContext db) : base(db, "Recipes"){ }

    public async Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Recipe>> GetRecipesPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
    {
        return await this._collection.Find(Builders<Recipe>.Filter.Empty)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Limit(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
    }

    public async Task<List<Recipe>> GetRecipesPageAsync(PageParameters pageParameters, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        return await this._collection.Find(predicate)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Limit(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
    }

    public async Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<Recipe>.Filter.Eq(x=>x.Id, recipe.Id), recipe, new ReplaceOptions(), cancellationToken);
    }
}
