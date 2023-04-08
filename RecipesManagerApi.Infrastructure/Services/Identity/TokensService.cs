using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Services.Identity;

public class TokensService : ITokensService
{
    private readonly IConfiguration _configuration;

    private readonly IUsersRepository _usersRepository;

    private readonly ILogger _logger;

    public TokensService(IConfiguration configuration, IUsersRepository usersRepository,
                            ILogger<TokensService> logger)
    {
        this._configuration = configuration;
        this._usersRepository = usersRepository;
        this._logger = logger;
    }

    public async Task<TokensModel> RefreshUserAsync(TokensModel tokensModel, CancellationToken cancellationToken)
    {
        var principal = this.GetPrincipalFromExpiredToken(tokensModel.AccessToken);

        var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value.ToString();
        var user = await this._usersRepository.GetUserAsync(u => u.Email == userId, cancellationToken);
        if (user == null || user?.RefreshToken != tokensModel.RefreshToken
            || user?.RefreshTokenExpiryDate <= DateTime.Now)
        {
            throw new SecurityTokenExpiredException();
        }

        var newAccessToken = this.GenerateAccessToken(principal.Claims);
        var newRefreshToken = this.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);

        this._logger.LogInformation($"Refreshed user tokens.");

        return new TokensModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<TokensModel> RefreshAppleGuestAsync(TokensModel tokensModel, CancellationToken cancellationToken)
    {
        var principal = this.GetPrincipalFromExpiredToken(tokensModel.AccessToken);

        var userId = Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await this._usersRepository.GetUserAsync(u => u.AppleDeviceId == userId, cancellationToken);
        if (user == null || user?.RefreshToken != tokensModel.RefreshToken
            || user?.RefreshTokenExpiryDate <= DateTime.Now)
        {
            throw new SecurityTokenExpiredException();
        }

        var newAccessToken = this.GenerateAccessToken(principal.Claims);
        var newRefreshToken = this.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);

        this._logger.LogInformation($"Refreshed apple guest tokens.");

        return new TokensModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<TokensModel> RefreshWebGuestAsync(TokensModel tokensModel, CancellationToken cancellationToken)
    {
        var principal = this.GetPrincipalFromExpiredToken(tokensModel.AccessToken);

        var userId = Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        var user = await this._usersRepository.GetUserAsync(u => u.WebId == userId, cancellationToken);
        if (user == null || user?.RefreshToken != tokensModel.RefreshToken
            || user?.RefreshTokenExpiryDate <= DateTime.Now)
        {
            throw new SecurityTokenExpiredException();
        }

        var newAccessToken = this.GenerateAccessToken(principal.Claims);
        var newRefreshToken = this.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);

        this._logger.LogInformation($"Refreshed web guest tokens.");

        return new TokensModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenOptions = GetTokenOptions(claims);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        this._logger.LogInformation($"Generated new access token.");

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            this._logger.LogInformation($"Generated new refresh token.");

            return refreshToken;
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey"))),
            ValidateLifetime = false 
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                                        StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        this._logger.LogInformation($"Returned data from expired access token.");

        return principal;
    }

    private JwtSecurityToken GetTokenOptions(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey")));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
            audience: _configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
            expires: DateTime.UtcNow.AddSeconds(30),
            claims: claims,
            signingCredentials: signinCredentials
        );

        return tokenOptions;
    }
}