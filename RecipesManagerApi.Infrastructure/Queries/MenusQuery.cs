using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class MenusQuery
{
	public Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
	[Service] IMenusService service)
	=> service.GetMenusPageAsync(pageNumber, pageSize, cancellationToken);
	
	public Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken,
	[Service] IMenusService service)
	=> service.GetMenuAsync(id, cancellationToken);
}