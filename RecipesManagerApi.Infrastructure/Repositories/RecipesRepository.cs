using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Threading;
using static Amazon.S3.Util.S3EventNotification;
using Amazon.S3.Model;
using RecipesManagerApi.Application.Models.LookUps;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class RecipesRepository : BaseRepository<Recipe>, IRecipesRepository
{
    public RecipesRepository(MongoDbContext db)
        : base(db, "Recipes") { }

    public async Task<Recipe> GetRecipeAsync(ObjectId id, CancellationToken cancellationToken)
    {
        return await (await this._collection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<RecipeLookUp>> GetRecipesPageAsync(int pageNumber, int pageSize, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var recipes = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .As<RecipeLookUp>()
            .ToListAsync(cancellationToken);

        return recipes;
    }

    public async Task<List<RecipeLookUp>> GetSavedRecipesAsync(int pageNumber, int pageSize, ObjectId userId, Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var recipes = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .Lookup("SavedRecipes", "_id", "RecipeId", "SavedRecipe")
            .AppendStage<RecipeLookUp>(new BsonDocument("$match", new BsonDocument("SavedRecipe.CreatedById", userId)))
            .AppendStage<RecipeLookUp>(new BsonDocument("$match", new BsonDocument("SavedRecipe.IsDeleted", false)))
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .As<RecipeLookUp>()
            .ToListAsync(cancellationToken);

        return recipes;
    }

    public async Task<int> GetSavedRecipesCountAsync(Expression<Func<Recipe, bool>> predicate, ObjectId userId, CancellationToken cancellationToken)
    {
        var pipeline = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .Lookup("SavedRecipes", "_id", "RecipeId", "SavedRecipe")
            .AppendStage<RecipeLookUp>(new BsonDocument("$match", new BsonDocument("SavedRecipe.CreatedById", userId)))
            .AppendStage<RecipeLookUp>(new BsonDocument("$match", new BsonDocument("SavedRecipe.IsDeleted", false)))
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
            .Count()
            .FirstOrDefaultAsync(cancellationToken);
        

        return (int)pipeline.Count;
    }

    public async Task<List<RecipeLookUp>> GetSubscribedRecipesAsync(int pageNumber, int pageSize, ObjectId userId,
        Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var recipes = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'Subscriptions',
                    localField: 'CreatedById',
                    foreignField: 'AuthorId',
                    as: 'Subscription',
                    pipeline: [
                        { 
                            $match: { 
                                $and: [
                                    { CreatedById: { $eq: ObjectId('" + userId.ToString() + @"') } },
                                    { IsDeleted: false }
                                ]
                            } 
                        }
                    ]
                }
            }")
            .AppendStage<BsonDocument>(@"
            {
                $match: {
                    Subscription: { $ne: [] } 
                }
            }")
            .AppendStage<BsonDocument>(@"
            { 
                $match: {
                    $expr: {
                        $cond: {
                            if: { $eq: [{ $arrayElemAt: ['$Subscription.IsAccessFull', 0] }, true] },
                            then: [],
                            else: { $eq: ['$$ROOT.IsPublic', true] }
                        }
                    }
                }
            }")
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("Subscription", 0)))
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .As<RecipeLookUp>()
            .ToListAsync(cancellationToken);

        return recipes;
    }

    public async Task<int> GetSubscriptionsCountAsync(Expression<Func<Recipe, bool>> predicate, ObjectId userId, CancellationToken cancellationToken)
    {
        var pipeline = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'Subscriptions',
                    localField: 'CreatedById',
                    foreignField: 'AuthorId',
                    as: 'Subscription',
                    pipeline: [
                        { 
                            $match: { 
                                $and: [
                                    { CreatedById: { $eq: ObjectId('" + userId.ToString() + @"') } },
                                    { IsDeleted: false }
                                ]
                            } 
                        }
                    ]
                }
            }")
            .AppendStage<BsonDocument>(@"
            {
                $match: {
                    Subscription: { $ne: [] } 
                }
            }")
            .AppendStage<BsonDocument>(@"
            { 
                $match: {
                    $expr: {
                        $cond: {
                            if: { $eq: [{ $arrayElemAt: ['$Subscription.IsAccessFull', 0] }, true] },
                            then: [],
                            else: { $eq: ['$$ROOT.IsPublic', true] }
                        }
                    }
                }
            }")
            .Count()
            .FirstOrDefaultAsync(cancellationToken);
        

        return (int)pipeline.Count;
    }

    public async Task UpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
    {
        await this._collection.ReplaceOneAsync(Builders<Recipe>.Filter.Eq(x=>x.Id, recipe.Id), recipe, new ReplaceOptions(), cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var filter = Builders<Recipe>.Filter.Where(predicate);
        return (int)(await this._collection.CountDocumentsAsync(filter, null, cancellationToken));
    }
}
