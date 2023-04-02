using AutoMapper;
using Microsoft.Extensions.Logging;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Models.Login;
using RecipesManagerApi.Application.Models.Register;
using RecipesManagerApi.Domain.Entities;
using System.Security.Claims;

namespace RecipesManagerApi.Infrastructure.Services.Identity;
public class UserManager : IUserManager
{
    private readonly IUsersRepository _usersRepository;

    private readonly ILogger _logger;

    private readonly IPasswordHasher _passwordHasher;

    private readonly ITokensService _tokensService;

    private readonly IMapper _mapper;

    private readonly IRolesRepository _rolesRepository;

    public UserManager(IUsersRepository usersRepository, ILogger<UserManager> logger, IPasswordHasher passwordHasher, ITokensService tokensService, IMapper mapper, IRolesRepository rolesRepository)
    {
        this._usersRepository = usersRepository;
        this._logger = logger;
        this._passwordHasher = passwordHasher;
        this._tokensService = tokensService;
        this._mapper = mapper;
        this._rolesRepository = rolesRepository;

    }

    public async Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken)
    {
        var user = await this._usersRepository.GetUserAsync(x => x.Email == login.Email, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        if (!this._passwordHasher.Check(login.Password, user.PasswordHash))
        {
            throw new InvalidDataException("Invalid password!");
        }

        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Logged in user with email: {login.Email}.");

        return tokens;
    }

    public async Task<TokensModel> LoginAppleGuestAsync(LoginAppleGuestModel login, CancellationToken cancellationToken)
    {
        var user = await this._usersRepository.GetUserAsync(x => x.AppleDeviceId == login.AppleDeviceId, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetAppleGuestTokens(user);

        this._logger.LogInformation($"Logged in apple guest with apple device id: {login.AppleDeviceId}.");

        return tokens;
    }

    public async Task<TokensModel> LoginWebGuestAsync(LoginWebGuestModel login, CancellationToken cancellationToken)
    {
        var user = await this._usersRepository.GetUserAsync(x => x.WebId == login.WebId, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetWebGuestTokens(user);

        this._logger.LogInformation($"Logged in web guest with web id: {login.WebId}.");

        return tokens;
    }

    public async Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken)
    {
        if (await this._usersRepository.ExistsAsync(u => u.Email == register.Email, cancellationToken))
        {
            throw new EntityAlreadyExistsException<User>("user email", register.Email);
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "User", cancellationToken);

        var user = new User
        {
            Name = register.Name,
            Email = register.Email,
            Roles = new List<Role> { role },
            PasswordHash = this._passwordHasher.Hash(register.Password),
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Created user with email: {user.Email}.");

        return tokens;
    }

    public async Task<TokensModel> RegisterWebGuestAsync(RegisterWebGuestModel register, CancellationToken cancellationToken)
    {
        if (await this._usersRepository.ExistsAsync(u => u.WebId == register.WebId, cancellationToken))
        {
            throw new EntityAlreadyExistsException<User>("guest Web Id", register.WebId.ToString());
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "Guest", cancellationToken);

        var user = new User
        {
            Name = "Guest",
            Roles = new List<Role> { role },
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(user, cancellationToken);
        var tokens = this.GetWebGuestTokens(user);

        this._logger.LogInformation($"Created web guest with web id: {user.WebId}.");

        return tokens;
    }

    public async Task<TokensModel> RegisterAppleGuestAsync(RegisterAppleGuestModel register, CancellationToken cancellationToken)
    {
        if (await this._usersRepository.ExistsAsync(u => u.AppleDeviceId == register.AppleDeviceId, cancellationToken))
        {
            throw new EntityAlreadyExistsException<User>("apple guest apple Id", register.AppleDeviceId.ToString());
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "Guest", cancellationToken);

        var user = new User
        {
            Name = register.Name,
            Roles = new List<Role> { role },
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(user, cancellationToken);
        var tokens = this.GetAppleGuestTokens(user);

        this._logger.LogInformation($"Created apple guest with apple device id: {user.AppleDeviceId}.");

        return tokens;
    }

    public async Task<TokensModel> AddToRoleAsync(string roleName, string email, CancellationToken cancellationToken)
    {
        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == roleName, cancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException<Role>();
        }

        var user = await this._usersRepository.GetUserAsync(x => x.Email == email, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        user.Roles.Add(role);
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Added role {roleName} to user with email: {email}.");

        return tokens;
    }

    public async Task<TokensModel> RemoveFromRole(string roleName, string email, CancellationToken cancellationToken)
    {
        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == roleName, cancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException<Role>();
        }

        var user = await this._usersRepository.GetUserAsync(x => x.Email == email, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        user.Roles.Remove(role);
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Added role {roleName} to user with email: {email}.");

        return tokens;
    }

    public async Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken)
    {
        var user = await this._usersRepository.GetUserAsync(x => x.Email == email, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        if (email != userDto.Email
            && await this._usersRepository.GetUserAsync(x => x.Email == userDto.Email, cancellationToken) != null)
        {
            throw new EntityAlreadyExistsException<User>("email", userDto.Email);
        }

        this._mapper.Map(user, userDto);
        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Update user with email: {email}.");

        return tokens;
    }

    private string GetRefreshToken()
    {
        var refreshToken = this._tokensService.GenerateRefreshToken();

        this._logger.LogInformation($"Returned new refresh token.");

        return refreshToken;
    }

    private TokensModel GetUserTokens(User user)
    {
        var claims = this.GetClaims(user);
        var accessToken = this._tokensService.GenerateAccessToken(claims);

        this._logger.LogInformation($"Returned new access and refresh tokens.");

        return new TokensModel
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken,
        };
    }

    private TokensModel GetAppleGuestTokens(User user)
    {
        var claims = this.GetAppleGuestClaims(user);
        var accessToken = this._tokensService.GenerateAccessToken(claims);

        this._logger.LogInformation($"Returned new access and refresh tokens.");

        return new TokensModel
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken,
        };
    }

    private TokensModel GetWebGuestTokens(User user)
    {
        var claims = this.GetWebGuestClaims(user);
        var accessToken = this._tokensService.GenerateAccessToken(claims);

        this._logger.LogInformation($"Returned new access and refresh tokens.");

        return new TokensModel
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken,
        };
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        this._logger.LogInformation($"Returned claims for user with email: {user.Email}.");

        return claims;
    }

    private IEnumerable<Claim> GetAppleGuestClaims(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.AppleDeviceId.ToString()),
            };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        this._logger.LogInformation($"Returned claims for user with email: {user.Email}.");

        return claims;
    }

    private IEnumerable<Claim> GetWebGuestClaims(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.WebId.ToString()),
            };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        this._logger.LogInformation($"Returned claims for user with email: {user.Email}.");

        return claims;
    }
}