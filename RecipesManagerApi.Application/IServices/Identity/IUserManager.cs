using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Models;

namespace RecipesManagerApi.Application.IServices.Identity;
public interface IUserManager
{
    Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

    Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);
}
