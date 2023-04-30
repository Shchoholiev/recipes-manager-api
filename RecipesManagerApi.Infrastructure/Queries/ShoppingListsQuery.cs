using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class ShoppingListsQuery
{
	public Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.GetShoppingListAsync(id , cancellationToken);
}