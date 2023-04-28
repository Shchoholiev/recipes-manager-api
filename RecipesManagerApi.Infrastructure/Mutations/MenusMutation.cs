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

	public Task<MenuDto> UpdateMenuAsync(MenuDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.UpdateMenuAsync(menuDto, cancellationToken);

	public Task DeleteMenuAsync(MenuDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.DeleteMenuAsync(menuDto, cancellationToken);
}