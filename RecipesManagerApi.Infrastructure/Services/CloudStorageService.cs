using System;
using System.Security.AccessControl;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using RecipesManagerApi.Application.IServices;
using SharpCompress.Common;
using Microsoft.AspNetCore.Http;
using RecipesManagerApi.Application.Exceptions;

namespace RecipesManagerApi.Infrastructure.Services;

public class CloudStorageService : ICloudStorageService
{
    private readonly string _bucketName;
    private readonly string _objectUrl;
    private readonly AmazonS3Client _s3Client;

    public CloudStorageService(IConfiguration configuration)
    {
        this._bucketName = configuration.GetSection("CloudObjectStorage")["BucketName"];
        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = configuration.GetConnectionString("StorageEndpoint");
        var accessKey = configuration.GetSection("CloudObjectStorage")["AccessKey"];
        var secretKey = configuration.GetSection("CloudObjectStorage")["SecretKey"];
        this._objectUrl = configuration.GetSection("CloudObjectStorage")["ObjectUrl"];
        this._s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task DeleteFileAsync(Guid guid, string fileExtension, CancellationToken cancellationToken)
    {
        var fileName = guid.ToString();
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = this._bucketName,
                Key = fileName + "." + fileExtension
            };

            Console.WriteLine($"Deleting object: {fileName}");
            await this._s3Client.DeleteObjectAsync(deleteObjectRequest);
            Console.WriteLine($"Object: {fileName} deleted from {this._bucketName}.");
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' when deleting an object.");
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file, Guid guid, string fileExtension, CancellationToken cancellationToken)
    {
        var fileName = guid.ToString()+ "." + fileExtension;
        using (var newMemoryStream = new MemoryStream())
        {
            file.CopyTo(newMemoryStream);

            var request = new PutObjectRequest()
            {
                BucketName = this._bucketName,
                Key = fileName,
                InputStream = newMemoryStream
            };


            var response = await this._s3Client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully uploaded {fileName} to {this._bucketName}.");
                
                var fileUrl = this._objectUrl + this._bucketName+ "/" + fileName;
                return fileUrl;
            }
            else
            {
                throw new UploadFileException(fileName, this._bucketName);
            }
        }
    }
}