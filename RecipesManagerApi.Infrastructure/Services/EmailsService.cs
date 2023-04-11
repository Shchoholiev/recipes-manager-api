using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using RecipesManagerApi.Application.Models.EmailModels;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IServices;

namespace RecipesManagerApi.Infrastructure.Services;

public class EmailsService : IEmailsService
{
	private readonly SmtpClient _smtpClient;
	private readonly IConfiguration _configuration;

	public EmailsService(IConfiguration configuration)
	{
		this._configuration = configuration;
		_smtpClient = new SmtpClient(this._configuration["SmtpHost"], Int32.Parse(this._configuration["SmtpPort"]))
		{
			Credentials = new NetworkCredential(this._configuration["SmtpUsername"], this._configuration["SmtpPassword"]),
			EnableSsl = true
		};
	}

	public async Task SendEmailMessageAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
	{
		try
		{
			MailMessage message = new MailMessage
			{
				From = new MailAddress(this._configuration["SmtpUsername"], this._configuration["EmailsSenderName"]),
				Subject = emailMessage.Subject,
				Body = emailMessage.Body,
				IsBodyHtml = true
			};

			foreach (var recipient in emailMessage.Recipients)
			{
				message.To.Add(recipient);
			}

			if (emailMessage.Attachments != null)
			{
				foreach (var attachment in emailMessage.Attachments)
				{
					message.Attachments.Add(new Attachment(new MemoryStream(attachment.Content), attachment.FileName, attachment.ContentType));
				}
			}

			await _smtpClient.SendMailAsync(message, cancellationToken);
		}
		catch (Exception ex)
		{
			throw new EmailsServiceException("Failed to send email message", ex);
		}
	}
}
