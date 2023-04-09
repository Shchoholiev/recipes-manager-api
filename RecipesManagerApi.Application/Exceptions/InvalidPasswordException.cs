namespace RecipesManagerApi.Application.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() { }

    public InvalidPasswordException(string password) : base(String.Format($"String {password} can not be a password.")) {}
}
