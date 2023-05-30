using MongoDB.Bson;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Application.IServices;

public interface IMenusService
{
	Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
	
	Task<MenuDto> GetMenuAsync(string menuId, CancellationToken cancellationToken);
	
	Task<MenuDto> AddMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task<MenuDto> UpdateMenuAsync(string id, MenuCreateDto dto, CancellationToken cancellationToken);
	
	Task<OperationDetails> DeleteMenuAsync (string menuId, CancellationToken cancellationToken);
	
	Task<OperationDetails> SendMenuToEmailsAsync(string menuId, IEnumerable<string> emailsTo, CancellationToken cancellationToken);
}
