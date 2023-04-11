using RecipesManagerApi.Application.IServices.Identity;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UserMutation
{
    public Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken,
        [Service] IUserManager userManager)
        => userManager.UpdateAsync(email, userDto, cancellationToken);
}
