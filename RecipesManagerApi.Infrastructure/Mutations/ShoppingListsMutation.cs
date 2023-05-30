using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class ShoppingListsMutation
{
	[Authorize]
	public Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.AddShoppingListAsync(shoppingList, cancellationToken);

	[Authorize]
	public Task<ShoppingListDto> UpdateShoppingListAsync(string id, ShoppingListCreateDto shoppingList, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.UpdateShoppingListAsync(id, shoppingList, cancellationToken);

	[Authorize]
	public Task<OperationDetails> DeleteShoppingListAsync(string id, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.DeleteShoppingListAsync(id, cancellationToken);

	[Authorize]
	public Task<OperationDetails> SendShoppingListToEmailsAsync(string shoppingListId, List<string> emails, CancellationToken cancellationToken,
	[Service] IShoppingListsService shoppingListsService)
	=> shoppingListsService.SendShoppingListToEmailsAsync(shoppingListId, emails, cancellationToken);
}