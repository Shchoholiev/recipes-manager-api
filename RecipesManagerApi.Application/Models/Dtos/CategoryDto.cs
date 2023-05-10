using HotChocolate;

namespace RecipesManagerApi.Application.Models.Dtos
{
    [GraphQLName("Category")]
    public class CategoryDto
    {
        public string Id { get; set; }

        //public string Name { get; set; }
    }
}
