using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Application.IServices
{
    public interface ICategoriesService
    {
        Task<CategoryDto> AddCategoryAsync(CategoryCreateDto dto, CancellationToken cancellationToken);

        Task<PagedList<CategoryDto>> GetCategoriesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken);
    }
}
