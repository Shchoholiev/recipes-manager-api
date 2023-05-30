using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Domain.Entities;
using Image = RecipesManagerApi.Domain.Entities.Image;
using System.Security.Cryptography;
using RecipesManagerApi.Domain.Enums;
using RecipesManagerApi.Application.GlodalInstances;

namespace RecipesManagerApi.Infrastructure.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesRepository _repository;

        private readonly IRecipesRepository _recipesRepository;

        private readonly IMapper _mapper;

        private readonly ICloudStorageService _cloudStorageService;

        public ImagesService(
            IImagesRepository repository,
            IRecipesRepository recipesRepository,
            IMapper mapper,
            ICloudStorageService cloudStorageService)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._cloudStorageService = cloudStorageService;
            this._recipesRepository = recipesRepository;
        }

        public async Task AddRecipeImageAsync(byte[] image, string imageExtension, ObjectId recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipesRepository.GetRecipeAsync(recipeId, cancellationToken);
            if (recipe == null)
            {
                throw new EntityNotFoundException<Recipe>();
            }

            var md5Hash = this.GetMd5Hash(image);
            var imageFromDb = await _repository.GetImageAsync(md5Hash, cancellationToken);
            if (imageFromDb == null) {
                var imageModel = new Domain.Entities.Image
                {
                    OriginalPhotoGuid = Guid.NewGuid(),
                    SmallPhotoGuid = Guid.NewGuid(),
                    Extension = imageExtension,
                    Md5Hash = md5Hash,
                    ImageUploadState = ImageUploadStates.Started,
                    CreatedById = GlobalUser.Id ?? ObjectId.Empty,
                    CreatedDateUtc = DateTime.UtcNow
                };
                await _repository.AddAsync(imageModel, cancellationToken);
                recipe.Thumbnail = imageModel;
                await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);

                try
                {
                    await Task.WhenAll(
                        _cloudStorageService.UploadFileAsync(image, imageModel.OriginalPhotoGuid, imageModel.Extension, cancellationToken),
                        ResizeAndUploadImageAsync(imageModel.SmallPhotoGuid, 600, image, imageModel, cancellationToken)
                    );
                    
                    imageModel.ImageUploadState = ImageUploadStates.Uploaded;
                }
                catch (Exception ex)
                {
                    imageModel.ImageUploadState = ImageUploadStates.Failed;
                    throw ex;
                }
                
                await _repository.UpdateAsync(imageModel, cancellationToken);
                await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);
            } else {
                recipe.Thumbnail = imageFromDb;
                await _recipesRepository.UpdateRecipeThumbnailAsync(recipeId, recipe, cancellationToken);
            }
        }

        private async Task ResizeAndUploadImageAsync(Guid guid, int width, byte[] image, Image imageModel, CancellationToken cancellationToken)
        {
            var resizedImage = this.ResizeImage(image, width);
            await _cloudStorageService.UploadFileAsync(resizedImage, guid, imageModel.Extension, cancellationToken);
        }

        private byte[] ResizeImage(byte[] imageBytes, int width)
        {
            using (var inputStream = new MemoryStream(imageBytes))
            using (var outputStream = new MemoryStream())
            {
                using (var image = SixLabors.ImageSharp.Image.Load(inputStream))
                {
                    var height = (int)(image.Height * ((float)width / image.Width));

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    }));
                    
                    image.Save(outputStream, image.Metadata.DecodedImageFormat);
                }

                return outputStream.ToArray();
            }
        }

        private string GetMd5Hash(byte[] image) {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(image);
            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return hashString;
        }
    }
}
