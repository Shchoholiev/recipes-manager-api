using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.Models;
public class UserDto
{
    public ObjectId Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryDate { get; set; }

    public Guid? AppleDeviceId { get; set; }

    public Guid? WebId { get; set; }

    public List<RoleDto> Roles { get; set; }
}
