using HotChocolate.Authorization;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class CategoriesQuery
{
    [Authorize]
    public Task<PagedList<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ICategoriesService service)
        => service.GetCategoriesPageAsync(pageNumber, pageSize, cancellationToken);

    public Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken, 
        [Service] ICategoriesService service)
        => service.GetCategoryAsync(id, cancellationToken);
}
