using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class MenusMutation
{
	[Authorize]
	public Task<MenuDto> AddMenuAsync(MenuCreateDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.AddMenuAsync(menuDto, cancellationToken);

	[Authorize]
	public Task<MenuDto> UpdateMenuAsync(string id, MenuCreateDto menuDto, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.UpdateMenuAsync(id, menuDto, cancellationToken);

	[Authorize]
	public Task<OperationDetails> DeleteMenuAsync(string menuId, CancellationToken cancellationToken,
		[Service] IMenusService service)
		=> service.DeleteMenuAsync(menuId, cancellationToken);
		
	[Authorize]
	public Task<OperationDetails> SendMenuToEmailsAsync(string menuId, List<string> emails, CancellationToken cancellationToken, 
		[Service] IMenusService service)
		=> service.SendMenuToEmailsAsync(menuId, emails, cancellationToken);	
}