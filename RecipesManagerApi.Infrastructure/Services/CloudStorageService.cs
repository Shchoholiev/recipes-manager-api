using System;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using RecipesManagerApi.Application.IServices;

namespace RecipesManagerApi.Infrastructure.Services;

public class CloudStorageService : ICloudStorageService
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _s3Client;

    public CloudStorageService(IConfiguration configuration)
    {
        this._bucketName = configuration.GetSection("CloudObjectStorage")["BucketName"];
        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = configuration.GetConnectionString("StorageEndpoint");
        var accessKey = configuration.GetSection("CloudObjectStorage")["AccessKey"];
        var secretKey = configuration.GetSection("CloudObjectStorage")["SecretKey"];
        this._s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task DeleteFileAsync(string fileName)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = this._bucketName,
                Key = fileName,
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

    public async Task<string> UploadFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}