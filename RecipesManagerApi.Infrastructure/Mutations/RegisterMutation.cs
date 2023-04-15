using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RegisterMutation
{
    public Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.RegisterAsync(register, cancellationToken);
}