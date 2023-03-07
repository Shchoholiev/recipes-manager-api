using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
