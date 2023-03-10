using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes; 

namespace RecipesManagerApi.Domain.Entities;

public class Ingredient
{
    [BsonId]
    public ObjectId Id { get; set; }

    public String Name { get; set; }

    public String? Units { get; set; }

    public Double? Amount { get; set; }
}
