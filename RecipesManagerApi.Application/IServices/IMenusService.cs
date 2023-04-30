using MongoDB.Bson;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Application.IServices;

public interface IMenusService
{
	Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
	
	Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken);
	
	Task<MenuDto> AddMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task<MenuDto> UpdateMenuAsync(MenuDto dto, CancellationToken cancellationToken);
	
	Task<OperationDetails> DeleteMenuAsync (MenuDto dto, CancellationToken cancellationToken);
	
	Task<OperationDetails> SendMenuToEmailAsync(string menuId, List<string> emailsTo, CancellationToken cancellationToken);
}
