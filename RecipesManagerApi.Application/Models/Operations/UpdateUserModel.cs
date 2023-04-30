using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Identity;

namespace RecipesManagerApi.Application.Models.Operations;

public class UpdateUserModel
{
    public TokensModel Tokens { get; set; }

    public UserDto User { get; set; }
}
