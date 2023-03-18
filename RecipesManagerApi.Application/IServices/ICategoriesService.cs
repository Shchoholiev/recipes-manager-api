using MongoDB.Bson;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IServices
{
    public interface ICategoriesService
    {
        Task AddCategoryAsync(CategoryDto dto, CancellationToken cancellationToken);

        Task<PagedList<CategoryDto>> GetCategoriesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken);
    }
}
