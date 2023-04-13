using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.Models.ExceptionHandling;
using System.Net;

namespace RecipesManagerApi.Api.CustomMiddlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    private readonly ILogger _logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        this._logger = logger;
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await this._next(httpContext);
        }
        catch (DeleteEntityException ex)
        {
            this._logger.LogError($"Can not delete entity: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.Conflict);
        }
        catch (EntityAlreadyExistsException ex)
        {
            this._logger.LogError($"Entity already exists: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.Conflict);
        }
        catch (EntityNotFoundException ex)
        {
            this._logger.LogError($"Entity not found: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.NotFound);
        }
        catch (InvalidEmailException ex)
        {
            this._logger.LogError($"Invalid value of email: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.BadRequest);
        }
        catch (InvalidPasswordException ex)
        {
            this._logger.LogError($"Invalid value of password: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.BadRequest);
        }
        catch (UploadFileException ex)
        {
            this._logger.LogError($"Can npt uplod the file: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.UnprocessableEntity);
        }
        catch (Exception ex)
        {
            this._logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex, (int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var message = exception switch
        {
            DeleteEntityException => $"{exception.Message} Can not delete this entity try again",
            EntityAlreadyExistsException => $"{exception.Message} Can not created this entity, it is already exists",
            EntityNotFoundException => $"{exception.Message} Can not found this entity, try again",
            InvalidPasswordException => $"{exception.Message} This password is too soft.",
            InvalidEmailException => $"{exception.Message} You need to pass valid email address.",
            UploadFileException => $"{exception.Message} Can not process this file",
            _ => "Internal Server Error",
        };

        await context.Response.WriteAsync(new ErrorDetails(statusCode, message).ToString());
    }
}
