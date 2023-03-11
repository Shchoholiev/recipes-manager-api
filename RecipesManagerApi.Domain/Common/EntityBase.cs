using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecipesManagerApi.Domain.Common;

public abstract class EntityBase
{
    [BsonId]
    public ObjectId Id { get; set; }

    public ObjectId CreatedById { get; set; }

    public DateTime CreatedDateUtc { get; set; }
}
