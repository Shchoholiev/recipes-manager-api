using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.Interfaces.Identity;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Identity;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Domain.Entities;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
        User user;
        if (login.Email != null)
        {
            user = await this._usersRepository.GetUserAsync(x => x.Email == login.Email, cancellationToken);
        }
        else
        {
            user = await this._usersRepository.GetUserAsync(x => x.Phone == login.Phone, cancellationToken);
        }

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

    public async Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken)
    {
        ValidatePassword(register.Password);
        if(register.Email != null) ValidateEmail(register.Email);
        if(register.Phone != null) ValidateNumber(register.Phone);

        if (register.Email != null)
        {
            if (await this._usersRepository.ExistsAsync(u => u.Email == register.Email, cancellationToken))
            {
                throw new EntityAlreadyExistsException<User>("user email", register.Email);
            }
        }
        else
        {
            if (await this._usersRepository.ExistsAsync(u => u.Phone == register.Phone, cancellationToken))
            {
                throw new EntityAlreadyExistsException<User>("user phone number", register.Phone);
            }
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "User", cancellationToken);

        var user = new User
        {
            Name = register.Name,
            Email = register.Email,
            Phone = register.Phone,
            Roles = new List<Role> { role },
            PasswordHash = this._passwordHasher.Hash(register.Password),
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Created user with email: {user.Email} and phone: {user.Phone}.");

        return tokens;
    }

    public async Task<TokensModel> AccessWebGuestAsync(AccessWebGuestModel register, CancellationToken cancellationToken)
    {
        var user = await this._usersRepository.GetUserAsync(x => x.WebId == register.WebId, cancellationToken);

        if (user != null)
        {
            user.RefreshToken = this.GetRefreshToken();
            await this._usersRepository.UpdateUserAsync(user, cancellationToken);
            var userTokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Logged in web guest with web id: {register.WebId}.");

            return userTokens;
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "Guest", cancellationToken);

        var newUser = new User
        {
            WebId = register.WebId,
            Name = "Guest",
            Roles = new List<Role> { role },
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(newUser, cancellationToken);
        var tokens = this.GetUserTokens(newUser);

        this._logger.LogInformation($"Created web guest with web id: {newUser.WebId}.");

        return tokens;
    }

    public async Task<TokensModel> AccessAppleGuestAsync(AccessAppleGuestModel register, CancellationToken cancellationToken)
    {
        if (await this._usersRepository.ExistsAsync(u => u.AppleDeviceId == register.AppleDeviceId, cancellationToken))
        {
            var user = await this._usersRepository.GetUserAsync(x => x.AppleDeviceId == register.AppleDeviceId, cancellationToken);

            user.RefreshToken = this.GetRefreshToken();
            await this._usersRepository.UpdateUserAsync(user, cancellationToken);
            var userTokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Logged in apple guest with device id: {register.AppleDeviceId}.");

            return userTokens;
        }

        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == "Guest", cancellationToken);

        var newUser = new User
        {
            AppleDeviceId = register.AppleDeviceId,
            Name = register.Name,
            Roles = new List<Role> { role },
            RefreshToken = this.GetRefreshToken(),
            RefreshTokenExpiryDate = DateTime.Now.AddDays(7),
        };

        await this._usersRepository.AddAsync(newUser, cancellationToken);
        var tokens = this.GetUserTokens(newUser);

        this._logger.LogInformation($"Created apple guest with apple device id: {newUser.AppleDeviceId}.");

        return tokens;
    }

    public async Task<TokensModel> AddToRoleAsync(string roleName, string id, CancellationToken cancellationToken)
    {
        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == roleName, cancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException<Role>();
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var user = await this._usersRepository.GetUserAsync(objectId, cancellationToken); 
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        user.Roles.Add(role);
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Added role {roleName} to user with id: {id}.");

        return tokens;
    }

    public async Task<TokensModel> RemoveFromRoleAsync(string roleName, string id, CancellationToken cancellationToken)
    {
        var role = await this._rolesRepository.GetRoleAsync(r => r.Name == roleName, cancellationToken);
        if (role == null)
        {
            throw new EntityNotFoundException<Role>();
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var user = await this._usersRepository.GetUserAsync(objectId, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        var deletedRole = user.Roles.Find(x => x.Name == role.Name);

        user.Roles.Remove(deletedRole);
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);
        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Added role {roleName} to user with id: {id}.");

        return tokens;
    }

    public async Task<UpdateUserModel> UpdateAsync(UserDto userDto, CancellationToken cancellationToken)
    {        
        if(userDto.Roles.Any(x => x.Name == "Guest"))
        {
            if(userDto.Password != null && (userDto.Email != null || userDto.Phone != null))
            {
                userDto.Roles.RemoveAll(x => x.Name == "Guest");
                var roleEntity = await this._rolesRepository.GetRoleAsync(x => x.Name == "User", cancellationToken);
                var roleDto = this._mapper.Map<RoleDto>(roleEntity);
                userDto.Roles.Add(roleDto);
            }
        }

        var user = await this._usersRepository.GetUserAsync(x => x.Id == GlobalUser.Id, cancellationToken);

        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        if (!userDto.Roles.Any(x => x.Name == "Guest") && userDto.Email != null)
        {
            if (await this._usersRepository.GetUserAsync(x => x.Email == userDto.Email, cancellationToken) != null)
            {
                throw new EntityAlreadyExistsException<User>("email", userDto.Email);
            }
        }
        if (!userDto.Roles.Any(x => x.Name == "Guest") && userDto.Phone != null)
        {
            if (await this._usersRepository.GetUserAsync(x => x.Phone == userDto.Phone, cancellationToken) != null)
            {
                throw new EntityAlreadyExistsException<User>("phone", userDto.Phone);
            }
        }

        this._mapper.Map(userDto, user);
        if (!userDto.Password.IsNullOrEmpty())
        {
            user.PasswordHash = this._passwordHasher.Hash(userDto.Password);
        }
        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);

        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Update user with id: {GlobalUser.Id.ToString()}.");

        return new UpdateUserModel() {Tokens = tokens, User = this._mapper.Map<UserDto>(user) };
    }

    public async Task<UpdateUserModel> UpdateUserByAdminAsync(string id, UserDto userDto, CancellationToken cancellationToken)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new InvalidDataException("Provided id is invalid.");
        }

        var user = await this._usersRepository.GetUserAsync(objectId, cancellationToken);

        if (user == null)
        {
            throw new EntityNotFoundException<User>();
        }

        this._mapper.Map(userDto, user);

        user.RefreshToken = this.GetRefreshToken();
        await this._usersRepository.UpdateUserAsync(user, cancellationToken);

        var tokens = this.GetUserTokens(user);

        this._logger.LogInformation($"Update user with id: {id}.");

        return new UpdateUserModel() { Tokens = tokens, User = this._mapper.Map<UserDto>(user) };
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

    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.Phone ?? string.Empty)
            };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        this._logger.LogInformation($"Returned claims for user with id: {user.Id.ToString()}.");

        return claims;
    }

    private void ValidatePassword(string password)
    {
        string regex = @"^(?=.*[a-zA-Z])(?=.*[\W_]).{8,}$";

        if (!Regex.IsMatch(password, regex))
        {
            throw new InvalidPasswordException(password);
        }
    }

    private void ValidateEmail(string email)
    {
        string regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, regex))
        {
            throw new InvalidEmailException(email);
        }
    }

    private void ValidateNumber(string phone)
    {
        string regex = @"^\+[0-9]{1,15}$";

        if (!Regex.IsMatch(phone, regex))
        {
            throw new InvalidPhoneNumberException(phone);
        }
    }
}