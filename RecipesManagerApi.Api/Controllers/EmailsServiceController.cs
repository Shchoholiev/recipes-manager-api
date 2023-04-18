using Microsoft.AspNetCore.Mvc;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.EmailModels;
using RecipesManagerApi.Infrastructure.Services;

namespace RecipesManagerApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailsServiceController : ControllerBase
{
	private readonly IEmailsService _emailsService;

	public EmailsServiceController(IEmailsService emailsService)
	{
		_emailsService = emailsService;
	}

	[HttpPost("send")]
	public async Task<IActionResult> SendEmailAsync([FromBody] EmailMessage emailMessage, CancellationToken cancellationToken)
	{
		try
		{
            await _emailsService.SendEmailMessageAsync(emailMessage, cancellationToken);
			return Ok();
		}
		catch(EmailSendException ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
	}
}
