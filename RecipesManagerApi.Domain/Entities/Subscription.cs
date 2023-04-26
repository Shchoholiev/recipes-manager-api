using RecipesManagerApi.Domain.Common;
using MongoDB.Bson;

namespace RecipesManagerApi.Domain.Entities;

public class Subscription : EntityBase
{
    public ObjectId AuthorId { get; set; }
    
    public bool IsAccessFull { get; set; }

    public bool IsDeleted { get; set; }
}
