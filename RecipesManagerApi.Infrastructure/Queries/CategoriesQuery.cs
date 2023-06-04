using HotChocolate.Authorization;
using MongoDB.Bson;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Paging;

namespace RecipesManagerApi.Infrastructure.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class CategoriesQuery
{
    [Authorize]
    public Task<PagedList<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken,
        [Service] ICategoriesService service)
        => service.GetCategoriesPageAsync(pageNumber, pageSize, cancellationToken);

    [Authorize]
    public Task<PagedList<CategoryDto>> SearchCategoriesAsync([Service] ICategoriesService service, 
        CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10, string search = "")
        => service.GetCategoriesPageAsync(pageNumber, pageSize, search, cancellationToken);

    [Authorize]
    public Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken, 
        [Service] ICategoriesService service)
        => service.GetCategoryAsync(id, cancellationToken);
}
