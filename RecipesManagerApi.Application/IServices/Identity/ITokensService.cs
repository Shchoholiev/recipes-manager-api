using RecipesManagerApi.Application.Models.Identity;
using System.Security.Claims;

namespace RecipesManagerApi.Application.Interfaces.Identity;

public interface ITokensService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    Task<TokensModel> RefreshAsync(TokensModel tokensModel, CancellationToken cancellationToken);
}