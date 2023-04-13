using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Application.IServices.Identity;

public interface IUserManager
{
    Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

    Task<TokensModel> AccessWebGuestAsync(AccessWebGuestModel register, CancellationToken cancellationToken);

    Task<TokensModel> AccessAppleGuestAsync(AccessAppleGuestModel register, CancellationToken cancellationToken);

    Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);

    Task<TokensModel> AddToRoleAsync(string roleName, string email, CancellationToken cancellationToken);

    Task<TokensModel> RemoveFromRoleAsync(string roleName, string email, CancellationToken cancellationToken);

    Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken);
}
