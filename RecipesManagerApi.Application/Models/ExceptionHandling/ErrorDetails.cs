using System.Text.Json;

namespace RecipesManagerApi.Application.Models.ExceptionHandling;

public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public ErrorDetails(int statusCode, string message)
    {
        this.StatusCode = statusCode;
        this.Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
