using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class Contact : EntityBase
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsImported { get; set; }
}
