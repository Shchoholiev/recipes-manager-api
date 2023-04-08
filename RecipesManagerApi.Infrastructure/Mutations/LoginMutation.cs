using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Models.Login;
using RecipesManagerApi.Application.Models.Register;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class LoginMutation
{
    public Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.LoginAsync(login, cancellationToken);
}
