using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class CategoriesMutation
{
    public Task<CategoryDto> AddCategoryAsync(CategoryCreateDto category, CancellationToken cancellationToken, 
        [Service] ICategoriesService service)
        => service.AddCategoryAsync(category, cancellationToken);
}
