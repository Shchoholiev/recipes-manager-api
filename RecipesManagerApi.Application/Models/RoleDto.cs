using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models;
public class RoleDto
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }
}
