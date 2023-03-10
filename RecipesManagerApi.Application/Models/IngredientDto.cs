using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes; 

namespace RecipesManagerApi.Application.Models;

public class IngredientDto
{
    [BsonId]
    public ObjectId Id { get; set; }

    public String Name { get; set; }

    public String? Units { get; set; }

    public Double? Amount { get; set; }
}
