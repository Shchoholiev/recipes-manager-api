using RecipesManagerApi.Application.Models.Identity;
using System.Security.Claims;

namespace RecipesManagerApi.Application.Interfaces.Identity;

public interface ITokensService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    Task<TokensModel> RefreshUserAsync(TokensModel tokensModel, CancellationToken cancellationToken);

    Task<TokensModel> RefreshAppleGuestAsync(TokensModel tokensModel, CancellationToken cancellationToken);

    Task<TokensModel> RefreshWebGuestAsync(TokensModel tokensModel, CancellationToken cancellationToken);
}