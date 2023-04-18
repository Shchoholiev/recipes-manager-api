namespace RecipesManagerApi.Application.Exceptions;

public class EmailSendException : Exception
{
	public EmailSendException(string message, Exception exception) 
	:base(message, exception)
	{
	}
}
