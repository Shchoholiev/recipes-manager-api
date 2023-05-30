using HotChocolate;

namespace RecipesManagerApi.Application.Models.Dtos;

public class ContactDto
{
    public string Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
    
}
