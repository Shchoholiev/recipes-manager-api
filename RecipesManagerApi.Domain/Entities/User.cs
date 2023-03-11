using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class User : EntityBase
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? PasswordHash { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryDate { get; set; }

    public Guid? AppleDeviceId { get; set; }

    public Guid? WebId { get; set; }

    public List<Role> Roles { get; set; }

    public bool IsDeleted { get; set; }
}

