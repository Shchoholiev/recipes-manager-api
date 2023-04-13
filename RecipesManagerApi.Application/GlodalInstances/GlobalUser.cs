using MongoDB.Bson;

namespace RecipesManagerApi.Application.GlodalInstances;
public static class GlobalUser
{
    public static ObjectId? Id { get; set; }

    public static string? Name { get; set; }

    public static string? Email { get; set; }
}
