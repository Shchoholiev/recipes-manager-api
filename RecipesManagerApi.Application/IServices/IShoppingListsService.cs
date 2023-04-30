using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Application.IServices;

public interface IShoppingListsService
{
	Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken);
	
	Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken);
	
	Task<ShoppingListDto> UpdateShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken);
	
	Task<OperationDetails> DeleteShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken);
	
	Task<OperationDetails> SendShoppingListToEmailAsync(string id, IEnumerable<string> emailsTo, CancellationToken cancellationToken);
}