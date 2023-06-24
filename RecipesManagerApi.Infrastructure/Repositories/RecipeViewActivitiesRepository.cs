using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using MongoDB.Bson;

namespace RecipesManagerApi.Infrastructure.Repositories;

public class RecipeViewActivitiesRepository : BaseRepository<RecipeViewActivity>, IRecipeViewActivitiesRepository
{
    public RecipeViewActivitiesRepository(MongoDbContext db) : base(db, "RecipeViewActivity") {}
}

