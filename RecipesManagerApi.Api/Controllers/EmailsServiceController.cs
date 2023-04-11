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
	private readonly IEmailsService _emailService;
	private readonly IConfiguration _configuration;

	public EmailsServiceController(IConfiguration configuration)
	{
		_configuration = configuration;
		_emailService = new EmailsService(_configuration);
	}

	[HttpPost("send")]
	public async Task<IActionResult> SendEmailAsync([FromBody] EmailMessage emailMessage)
	{
		try
		{
            await _emailService.SendEmailMessageAsync(emailMessage, new CancellationToken());
			return Ok();
		}
		catch(EmailsServiceException ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
	}
}
