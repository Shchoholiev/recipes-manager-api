using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UserMutation
{
    [Authorize]
    public Task<UpdateUserModel> UpdateUserAsync(string email, UserDto userDto, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.UpdateAsync(email, userDto, cancellationToken);
}
