using HotChocolate;

namespace RecipesManagerApi.Application.Models
{
    [GraphQLName("Category")]
    public class CategoryDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
