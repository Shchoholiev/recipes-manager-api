using System;

namespace RecipesManagerApi.Infrastructure.Email
{
    public class EmailServiceException : Exception
    {
        public EmailServiceException(string message, Exception exception) 
        :base(message, exception)
        {
        }
    }
}