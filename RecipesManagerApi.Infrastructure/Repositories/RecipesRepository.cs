using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
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

    public async Task<RecipeLookUp> GetRecipeAsync(ObjectId id, ObjectId userId, CancellationToken cancellationToken)
    {
        var recipe = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(r => r.Id == id))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'SavedRecipes',
                    let: { recipeId: '$_id', userId: ObjectId('" + userId.ToString() + @"') },
                    pipeline: [
                        { 
                            $match: { 
                                $and: [
                                    { $expr: { $eq: ['$CreatedById', '$$userId'] } },
                                    { $expr: { $eq: ['$RecipeId', '$$recipeId'] } },
                                    { IsDeleted: false }
                                ]
                            } 
                        }
                    ],
                    as: 'SavedRecipe',
                }
            }")
            .AppendStage<BsonDocument>(@"
            {
                $addFields: {
                    IsSaved: {
                        $cond: {
                            if: { $eq: [{ $size: '$SavedRecipe' }, 0] },
                            then: false,
                            else: true
                        }
                    }
                }
            }")
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
            .As<RecipeLookUp>()
            .FirstOrDefaultAsync(cancellationToken);

        return recipe;
    }

    public async Task<List<RecipeLookUp>> GetRecipesPageAsync(int pageNumber, int pageSize, ObjectId userId, 
        Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var id = userId.ToString();
        var recipes = await _collection.Aggregate()
            .Match(Builders<Recipe>.Filter.Where(predicate))
            .Lookup("Users", "CreatedById", "_id", "CreatedBy")
            .AppendStage<RecipeLookUp>("{ $addFields: { CreatedBy: { $arrayElemAt: ['$CreatedBy', 0] } } }")
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'SavedRecipes',
                    let: { recipeId: '$_id', userId: ObjectId('" + userId.ToString() + @"') },
                    pipeline: [
                        { 
                            $match: { 
                                $and: [
                                    { $expr: { $eq: ['$CreatedById', '$$userId'] } },
                                    { $expr: { $eq: ['$RecipeId', '$$recipeId'] } },
                                    { IsDeleted: false }
                                ]
                            } 
                        }
                    ],
                    as: 'SavedRecipe',
                }
            }")
            .AppendStage<BsonDocument>(@"
            {
                $addFields: {
                    IsSaved: {
                        $cond: {
                            if: { $eq: [{ $size: '$SavedRecipe' }, 0] },
                            then: false,
                            else: true
                        }
                    }
                }
            }")
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
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
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'SavedRecipes',
                    localField: '_id',
                    foreignField: 'RecipeId',
                    as: 'SavedRecipe',
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
                    SavedRecipe: { $ne: [] } 
                }
            }")
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
            .AppendStage<BsonDocument>(@"
            {
                $addFields: {
                    IsSaved: true
                }
            }")
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
        

        return (int)(pipeline?.Count ?? 0);
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
            .AppendStage<BsonDocument>(@"
            { 
                $lookup: { 
                    from: 'SavedRecipes',
                    let: { recipeId: '$_id', userId: ObjectId('" + userId.ToString() + @"') },
                    pipeline: [
                        { 
                            $match: { 
                                $and: [
                                    { $expr: { $eq: ['$CreatedById', '$$userId'] } },
                                    { $expr: { $eq: ['$RecipeId', '$$recipeId'] } },
                                    { IsDeleted: false }
                                ]
                            } 
                        }
                    ],
                    as: 'SavedRecipe',
                }
            }")
            .AppendStage<BsonDocument>(@"
            {
                $addFields: {
                    IsSaved: {
                        $cond: {
                            if: { $eq: [{ $size: '$SavedRecipe' }, 0] },
                            then: false,
                            else: true
                        }
                    }
                }
            }")
            .AppendStage<RecipeLookUp>(new BsonDocument("$project", new BsonDocument("SavedRecipe", 0)))
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
        
        return (int)(pipeline?.Count ?? 0);
    }

    public async Task<Recipe> UpdateRecipeAsync(ObjectId id, Recipe recipe, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Recipe>.Update
            .Set(r => r.Name, recipe.Name)
            .Set(r => r.Text, recipe.Text)
            .Set(r => r.Ingredients, recipe.Ingredients)
            .Set(r => r.IngredientsText, recipe.IngredientsText)
            .Set(r => r.Categories, recipe.Categories)
            .Set(r => r.Calories, recipe.Calories)
            .Set(r => r.ServingsCount, recipe.ServingsCount)
            .Set(r => r.IsPublic, recipe.IsPublic)
            .Set(r => r.LastModifiedById, recipe.LastModifiedById)
            .Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc);
        
        var options = new FindOneAndUpdateOptions<Recipe>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await this._collection.FindOneAndUpdateAsync(
            Builders<Recipe>.Filter.Eq(r => r.Id, id), updateDefinition, options, cancellationToken);
    }

    public async Task<Recipe> UpdateRecipeThumbnailAsync(ObjectId id, Recipe recipe, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Recipe>.Update
            .Set(r => r.Thumbnail, recipe.Thumbnail)
            .Set(r => r.LastModifiedById, recipe.LastModifiedById)
            .Set(r => r.LastModifiedDateUtc, recipe.LastModifiedDateUtc);
        
        var options = new FindOneAndUpdateOptions<Recipe>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await this._collection.FindOneAndUpdateAsync(
            Builders<Recipe>.Filter.Eq(r => r.Id, id), updateDefinition, options, cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(Expression<Func<Recipe, bool>> predicate, CancellationToken cancellationToken)
    {
        var filter = Builders<Recipe>.Filter.Where(predicate);
        return (int)(await this._collection.CountDocumentsAsync(filter, null, cancellationToken));
    }
}
