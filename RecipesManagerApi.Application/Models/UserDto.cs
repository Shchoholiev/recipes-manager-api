using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models;
public class UserDto
{
    public string Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryDate { get; set; }

    public Guid? AppleDeviceId { get; set; }

    public Guid? WebId { get; set; }

    public List<RoleDto> Roles { get; set; }
}
