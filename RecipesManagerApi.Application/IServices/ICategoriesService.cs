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

        Task<PagedList<CategoryDto>> GetCategoriesPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<CategoryDto> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken);
    }
}
