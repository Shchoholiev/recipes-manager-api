using System;
namespace RecipesManagerApi.Application.Exceptions;

public class UploadFileException : Exception
{
	public UploadFileException()
	{
	}

	public UploadFileException(string fileName, string bucketName) : base(String.Format($"Could not upload {fileName} to {bucketName}."))
	{
	}
}


