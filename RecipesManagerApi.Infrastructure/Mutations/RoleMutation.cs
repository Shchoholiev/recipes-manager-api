using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RoleMutation
{
    public Task<TokensModel> AddToRoleAsync(string roleName, string email, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.AddToRoleAsync(roleName, email, cancellationToken);

    public Task<TokensModel> RemoveFromRoleAsync(string roleName, string email, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.RemoveFromRoleAsync(roleName, email, cancellationToken);

    public Task<RoleDto> AddRole(RoleCreateDto roleDto, CancellationToken cancellationToken,
        [Service] IRolesService rolesService)
        => rolesService.AddRoleAsync(roleDto, cancellationToken);
}