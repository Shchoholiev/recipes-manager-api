using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace RecipesManagerApi.Application.IServices
{
    public interface IImagesService
    {
        Task AddRecipeImageAsync(byte[] image, string imageExtension, ObjectId recipeId, CancellationToken cancellationToken);
    }
}
