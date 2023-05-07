using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class ShoppingListsQuery
{
	public Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.GetShoppingListAsync(id , cancellationToken);
	
	public Task<PagedList<ShoppingListDto>> GetShoppingListsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.GetShoppingListsPageAsync(pageNumber, pageSize, cancellationToken);
}