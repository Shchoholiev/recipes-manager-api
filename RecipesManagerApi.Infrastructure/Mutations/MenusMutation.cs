using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class MenusMutation
{
	public Task<MenuDto> AddMenuAsync(MenuCreateDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.AddMenuAsync(menuDto, cancellationToken);
}