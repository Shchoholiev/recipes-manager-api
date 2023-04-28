using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices;

public interface IMenusService
{
	Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
	
	Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken);
	
	Task<MenuDto> AddMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task<MenuDto> UpdateMenuAsync(MenuDto dto, CancellationToken cancellationToken);
	
	Task DeleteMenuAsync (MenuDto dto, CancellationToken cancellationToken);
	
	Task SendMenuToEmailAsync(string menuId, List<string> emailsTo, CancellationToken cancellationToken);
}
