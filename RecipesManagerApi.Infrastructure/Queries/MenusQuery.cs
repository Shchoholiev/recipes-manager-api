using HotChocolate.Authorization;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class MenusQuery
{
	[Authorize]
	public Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
	[Service] IMenusService service)
	=> service.GetMenusPageAsync(pageNumber, pageSize, cancellationToken);
	
	[Authorize]
	public Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken,
	[Service] IMenusService service)
	=> service.GetMenuAsync(id, cancellationToken);
}