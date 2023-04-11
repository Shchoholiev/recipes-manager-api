using RecipesManagerApi.Application.Models.EmailModels;

namespace RecipesManagerApi.Application.IServices;

public interface IEmailsService
{
	Task SendEmailMessageAsync(EmailMessage emailMessage, CancellationToken cancellationToken);
}