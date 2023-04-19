using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IMenusService
{
	public Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken);
	
	Task<MenuDto> GetMenuAsync(ObjectId id, CancellationToken cancellationToken);
	
	Task AddMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task UpdateMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task<bool> SendMenuToEmailAsync(ObjectId menuId, List<string> emailsTo, CancellationToken cancellationToken);
}
