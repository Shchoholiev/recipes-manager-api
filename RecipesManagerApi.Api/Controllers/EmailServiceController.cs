using Microsoft.AspNetCore.Mvc;
using RecipesManagerApi.Infrastructure.Email;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipesManagerApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailServiceController : ControllerBase
{
	private readonly EmailService _emailService;
	private readonly IConfiguration _configuration;

	public EmailServiceController(IConfiguration configuration)
	{
		_configuration = configuration;
		_emailService = new EmailService(_configuration);
	}

	[HttpPost("send")]
	public async Task<IActionResult> SendEmailAsync([FromBody] EmailMessage emailMessage)
	{
		try
		{
            await _emailService.SendEmailMessageAsync(emailMessage);
			return Ok();
		}
		catch(EmailServiceException ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
	}
}
