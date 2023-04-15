using HotChocolate;

namespace RecipesManagerApi.Application.Models.CreateDtos;

[GraphQLName("ContactInput")]
public class ContactCreateDto
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}
