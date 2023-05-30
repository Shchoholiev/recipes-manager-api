using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IShoppingListsService
{
	Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken);
	
	Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken);
	
	Task<ShoppingListDto> UpdateShoppingListAsync(string id, ShoppingListCreateDto shoppingList, CancellationToken cancellationToken);
	
	Task<OperationDetails> DeleteShoppingListAsync(string id, CancellationToken cancellationToken);
	
	Task<PagedList<ShoppingListDto>> GetShoppingListsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
	
	Task<OperationDetails> SendShoppingListToEmailsAsync(string id, IEnumerable<string> emailsTo, CancellationToken cancellationToken);
}