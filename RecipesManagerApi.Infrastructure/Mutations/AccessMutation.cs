using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AccessMutation
{
    public Task<TokensModel> AccessWebGuestAsync(AccessWebGuestModel model, CancellationToken cancellationToken,
    [Service] IUserManager userManager)
    => userManager.AccessWebGuestAsync(model, cancellationToken);

    public Task<TokensModel> AccessAppleGuestAsync(AccessAppleGuestModel model, CancellationToken cancellationToken,
    [Service] IUserManager userManager)
    => userManager.AccessAppleGuestAsync(model, cancellationToken);

    public Task<TokensModel> RefreshUserTokenAsync(TokensModel model, CancellationToken cancellationToken,
    [Service] ITokensService tokensService)
    => tokensService.RefreshUserAsync(model, cancellationToken);
}