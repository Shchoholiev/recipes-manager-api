using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Threading;
using static Amazon.S3.Util.S3EventNotification;
using Amazon.S3.Model;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class RecipesRepository : BaseRepository<Recipe>, IRecipesRepository
{
    public RecipesRepository(MongoDbContext db)
        : base(db, "Recipes") { }

    public async Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<Recipe>.Filter.Eq(x=>x.Id, recipe.Id), recipe, new ReplaceOptions(), cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate)
    {
        var filter = Builders<Recipe>.Filter.Where(predicate);
        return (int)(await this._collection.CountDocumentsAsync(filter));
    }

    public async Task<List<Recipe>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, List<Subscription> subscriptions,
        Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        List<ObjectId> subscriptionsIds = new List<ObjectId>();

        foreach (var s in subscriptions)
        {
            subscriptionsIds.Add(s.AuthorId);
        }

        var filterSubscribed = Builders<Recipe>.Filter.In(Recipe => Recipe.CreatedById, subscriptionsIds);
        var filterPredicate = Builders<Recipe>.Filter.Where(predicate);
        var filter = Builders<Recipe>.Filter.And(filterSubscribed, filterPredicate);

        return await this._collection.Find(filter)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Limit(pageSize)
                                         .ToListAsync(cancellationToken);
    }

    public async Task<int> GetSubscriptionsCountAsync(Expression<Func<Recipe, bool>> predicate, List<Subscription> subscriptions, CancellationToken cancellationToken)
    {
        List<ObjectId> subscriptionsIds = new List<ObjectId>();

        foreach (var s in subscriptions)
        {
            subscriptionsIds.Add(s.Id);
        }

        var filterSubscribed = Builders<Recipe>.Filter.In(Recipe => Recipe.CreatedById, subscriptionsIds);
        var filterPredicate = Builders<Recipe>.Filter.Where(predicate);
        var filter = Builders<Recipe>.Filter.And(filterSubscribed, filterPredicate);

        return (int)(await this._collection.CountDocumentsAsync(filter));
    }

    public async Task<List<Recipe>> GetSavedRecipesAsync(int pageNumber, int pageSize,Expression<Func<Recipe, bool>> predicate, List<SavedRecipe> saves, CancellationToken cancellationToken)
    {
        List<ObjectId> savedRecipesIds = new List<ObjectId>();

        foreach(var s in saves)
        {
            savedRecipesIds.Add(s.RecipeId);
        }

        var filterSavedRecipes = Builders<Recipe>.Filter.In(Recipe => Recipe.Id, savedRecipesIds);
        var filterPredicate = Builders<Recipe>.Filter.Where(predicate);
        var filter = Builders<Recipe>.Filter.And(filterSavedRecipes, filterPredicate);

        return await this._collection.Find(filter)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync(cancellationToken);
    }

    public async Task<int> GetSavedRecipesCountAsync(Expression<Func<Recipe, bool>> predicate, List<SavedRecipe> saves, CancellationToken cancellationToken)
    {
        List<ObjectId> savedRecipesIds = new List<ObjectId>();

        foreach (var s in saves)
        {
            savedRecipesIds.Add(s.RecipeId);
        }

        var filterSavedRecipes = Builders<Recipe>.Filter.In(Recipe => Recipe.Id, savedRecipesIds);
        var filterPredicate = Builders<Recipe>.Filter.Where(predicate);
        var filter = Builders<Recipe>.Filter.And(filterSavedRecipes, filterPredicate);

        return (int)(await this._collection.CountDocumentsAsync(filter));
    }
}
