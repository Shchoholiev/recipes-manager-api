using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class ShoppingListsMutation
{
	public Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.AddShoppingListAsync(shoppingList, cancellationToken);

	public Task<ShoppingListDto> UpdateShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.UpdateShoppingListAsync(shoppingList, cancellationToken);

	public Task<OperationDetails> DeleteShoppingListAsync(ShoppingListDto shoppingList, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.DeleteShoppingListAsync(shoppingList, cancellationToken);
}