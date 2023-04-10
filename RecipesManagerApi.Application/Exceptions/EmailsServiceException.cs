namespace RecipesManagerApi.Application.Exceptions;

public class EmailsServiceException : Exception
{
	public EmailsServiceException(string message, Exception exception) 
	:base(message, exception)
	{
	}
}
