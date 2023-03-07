using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Database;

namespace RecipesManagerApi.Infrastructure.DataInitializers;

public class MongoDbInitializer
{
    public async Task Initialize(MongoDbContext context) {
        var db = context.Db;

        var categoriesCollection = db.GetCollection<Category>("Categories");

        var pizza = new Category {
            Name = "Pizza",
            CreatedDateUtc = DateTime.UtcNow
        };

        await categoriesCollection.InsertOneAsync(pizza);
    }
}
