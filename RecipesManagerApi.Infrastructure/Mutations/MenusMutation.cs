using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class MenusMutation
{
	public Task<MenuDto> AddMenuAsync(MenuCreateDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.AddMenuAsync(menuDto, cancellationToken);

	public Task<MenuDto> UpdateMenuAsync(MenuCreateDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.UpdateMenuAsync(menuDto, cancellationToken);

	public Task<OperationDetails> DeleteMenuAsync(string menuId, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.DeleteMenuAsync(menuId, cancellationToken);
}