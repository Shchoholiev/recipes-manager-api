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
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    private readonly ISavedRecipesRepository _savedRecipesRepository;

    public RecipesRepository(MongoDbContext db, ISubscriptionsRepository subscriptionsRepository, ISavedRecipesRepository savedRecipesRepository)
        : base(db, "Recipes"){
        this._subscriptionsRepository = subscriptionsRepository;
        this._savedRecipesRepository = savedRecipesRepository;
    }

    public async Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Recipe>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, ObjectId id, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var subscriptions = await this._subscriptionsRepository.GetUsersSubscriptionsAsync(id, cancellationToken);
        IEnumerable<ObjectId> subscriptionsIds = new List<ObjectId>();

        foreach(var s in subscriptions)
        {
            subscriptionsIds.Append<ObjectId>(s.Id);
        }

        var filter = Builders<Recipe>.Filter.In(Recipe => Recipe.CreatedById, subscriptionsIds) & Builders<Recipe>.Filter.Where(predicate);

        return await this._collection.Find(filter)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Limit(pageSize)
                                         .ToListAsync(cancellationToken);
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

    public async Task<int> GetSubscriptionsCountAsync(ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var subscriptions = await this._subscriptionsRepository.GetUsersSubscriptionsAsync(userId, cancellationToken);
        IEnumerable<ObjectId> subscriptionsIds = new List<ObjectId>();

        foreach (var s in subscriptions)
        {
            subscriptionsIds.Append<ObjectId>(s.Id);
        }

        var filterSubscriptions = Builders<Recipe>.Filter.In(Recipe => Recipe.CreatedById, subscriptionsIds) & Builders<Recipe>.Filter.Where(predicate);
        return (int)(await this._collection.CountDocumentsAsync(filterSubscriptions));
    }

    public async Task<List<Recipe>> GetSavedRecipesAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var saves = await this._savedRecipesRepository.GetUsersSavesAsync(userId, cancellationToken);
        IEnumerable<ObjectId> savedRecipesIds = new List<ObjectId>();

        foreach(var s in saves)
        {
            savedRecipesIds.Append(s.RecipeId);
        }

        var filterSavedRecipes = Builders<Recipe>.Filter.In(Recipe => Recipe.Id, savedRecipesIds) & Builders<Recipe>.Filter.Where(predicate);

        return await this._collection.Find(filterSavedRecipes)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync(cancellationToken);
    }

    public async Task<int> GetSavedRecipesCountAsync(ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var saves = await this._savedRecipesRepository.GetUsersSavesAsync(userId, cancellationToken);
        IEnumerable<ObjectId> savedRecipesIds = new List<ObjectId>();

        foreach (var s in saves)
        {
            savedRecipesIds.Append(s.RecipeId);
        }

        var filterSavedRecipes = Builders<Recipe>.Filter.In(Recipe => Recipe.Id, savedRecipesIds) & Builders<Recipe>.Filter.Where(predicate);

        return (int)(await this._collection.CountDocumentsAsync(filterSavedRecipes));
    }
}
