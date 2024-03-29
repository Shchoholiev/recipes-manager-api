using System.Text;
using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.EmailModels;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Application.Models.Operations;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Services;

public class MenusService : IMenusService
{
	private readonly IMenusRepository _menusRepository;
	private readonly IMapper _mapper;
	private readonly IEmailsService _emailsService;
	private readonly IContactsRepository _contactsRepository;
	
	public MenusService(
		IMenusRepository menusRepository,
		IMapper mapper,
		IEmailsService emailService,
		IContactsRepository contactsRepository)
	{
		this._menusRepository = menusRepository;
		this._mapper = mapper;
		this._emailsService = emailService;
		this._contactsRepository = contactsRepository;
	}
	
	public async Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		var userId = GlobalUser.Id.Value;
		var entities = await this._menusRepository.GetPageAsync(pageNumber, pageSize, userId, cancellationToken);
		var dtos = this._mapper.Map<List<MenuDto>>(entities);
		var count = await this._menusRepository.GetTotalCountAsync(x => !x.IsDeleted && x.CreatedById == userId);
		return new PagedList<MenuDto>(dtos, pageNumber, pageSize, count);
	}
	
	public async Task<MenuDto> GetMenuAsync(string menuId, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(menuId, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var entity = await this._menusRepository.GetMenuLookedUpAsync(objectId, cancellationToken);
		if(entity == null)
		{
			throw new EntityNotFoundException<Menu>();
		}
		return this._mapper.Map<MenuDto>(entity);
	}
	
	public async Task<MenuDto> AddMenuAsync(MenuCreateDto createDto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Menu>(createDto);
		entity.CreatedById = GlobalUser.Id.Value;
		entity.CreatedDateUtc = DateTime.UtcNow;
		var menu = await this._menusRepository.AddMenuAsync(entity, cancellationToken);
		return this._mapper.Map<MenuDto>(menu);
	}
	
	public async Task<MenuDto> UpdateMenuAsync(string id, MenuCreateDto createDto, CancellationToken cancellationToken)
	{
		if(!ObjectId.TryParse(id, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var entity = this._mapper.Map<Menu>(createDto);
		entity.LastModifiedById = GlobalUser.Id.Value;
		entity.LastModifiedDateUtc = DateTime.UtcNow;
		var entityLookedUp = await this._menusRepository.UpdateMenuAsync(objectId, entity, cancellationToken);
		var dto = this._mapper.Map<MenuDto>(entityLookedUp);
		return dto;
	}
	
	public async Task<OperationDetails> DeleteMenuAsync(string menuId, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(menuId, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		
		var menu = new Menu
		{
			Id = objectId,
			LastModifiedById = GlobalUser.Id.Value,
			LastModifiedDateUtc = DateTime.UtcNow	
		};
		
		await this._menusRepository.DeleteAsync(menu, cancellationToken);
		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}
	
	public async Task<OperationDetails> SendMenuToEmailsAsync(string menuId, IEnumerable<string> emailsTo, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(menuId, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var menuLookedUp = await this._menusRepository.GetMenuLookedUpAsync(objectId, cancellationToken);
		if(menuLookedUp == null)
		{
			throw new EntityNotFoundException<Menu>();
		}
		var menuDto = this._mapper.Map<MenuDto>(menuLookedUp);
		var message = new EmailMessage
		{
			Recipients = emailsTo.ToList(),
			Subject = $"{menuDto.Name}:",
			Body = FormMenuEmailHTMLBody(menuDto)
		};
		
		await Task.WhenAll(
			this.CheckContacsAsync(menuLookedUp, emailsTo, cancellationToken),
			this._emailsService.SendEmailMessageAsync(message, cancellationToken)
		);

		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}
	
	private async Task CheckContacsAsync(MenuLookedUp menuLookedUp, IEnumerable<string> emailsTo, CancellationToken cancellationToken)
	{
		if (!emailsTo.Any()) return;

		var menu = this._mapper.Map<Menu>(menuLookedUp);
		var existingContacts = await this._contactsRepository.GetPageAsync(1, 100, 
			c => c.CreatedById == GlobalUser.Id.Value && c.IsDeleted == false && emailsTo.Contains(c.Email), cancellationToken);

		foreach (var email in emailsTo)
		{
			var contact = existingContacts.Where(x => x.Email == email).FirstOrDefault();
			if (contact == null)
			{
				contact = new Contact
				{
					Email = email,
					CreatedById = GlobalUser.Id.Value,
					CreatedDateUtc = DateTime.UtcNow
				};
				await this._contactsRepository.AddAsync(contact, cancellationToken);
			}
			menu.SentTo.Add(contact.Id);
			menu.LastModifiedById = GlobalUser.Id.Value;
			menu.LastModifiedDateUtc = DateTime.UtcNow;
		}
		
		await this._menusRepository.UpdateMenuSentToAsync(menu, cancellationToken);
	}	
	
	private string FormMenuEmailHTMLBody(MenuDto menu)
	{
		StringBuilder sb = new StringBuilder(@"
		<!DOCTYPE html>
		<html lang=""en"">
		<head>
  			<meta charset=""UTF-8"">
  			<style>
				table, td, div, h1, p {font-family: Arial, sans-serif;}
  			</style>
		</head>
		");
		sb.Append($@"
			<body style=""margin:0;padding:0;"">
  <table role=""presentation"" style=""width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;"">
	<tr>
	  <td align=""center"" style=""padding:0;"">
		<table role=""presentation"" style=""width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;"">
		  <tr>
			<td align=""center"" style=""padding:40px 0 30px 0;background:#ffa500;"">
			  <h1 style=""font"">Recipes Manager</h1>
			</td>
		  </tr>
		  <tr>
			<td style=""padding:36px 30px 42px 30px;"">
			  <table role=""presentation"" style=""width:100%;border-collapse:collapse;border:0;border-spacing:0;"">
				<tr>
				  <td style=""padding:0;color:#153643;"">
					<h1 style=""font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;text-align:center"">{menu.Name}</h1>
				  </td>
				</tr>
		");
		
		if(menu.Recipes != null && menu.Recipes.Count > 0)
		{
			foreach (var recipe in menu.Recipes)
			{
				sb.Append(@$"
				<tr>
				<td>
				<p style=""margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;"">Recipe: {recipe.Name} for {recipe.ServingsCount} serving(s):</p>
				");
				sb.Append($"<p>- Calories: {recipe.Calories}</p>");
				sb.Append("</td></tr>");
			}
		}
		else
		{
			sb.Append(@"
				<tr>
					<td><h1 style=""color:#153643;font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;text-align:center"">This menu contains no recipes</h1></td>
				</tr>
			");
		}
		
		sb.Append(@"
			</table>
			</td>
		  </tr>
		  
		  <tr>
			<td style=""padding:30px;background:#ee4c50;"">
			  <table role=""presentation"" style=""width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;"">
				<tr>
				  <td style=""padding:0;width:50%;"" align=""left"">
					<p style=""margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;"">
					  &reg; Recipes Manager<br/><a href="""" style=""color:#ffffff;text-decoration:underline;"">Main Page</a>
					</p>
				  </td>
				  <td style=""padding:0;width:50%;"" align=""right"">
					<table role=""presentation"" style=""border-collapse:collapse;border:0;border-spacing:0;"">
					  <tr>
						<td style=""padding:0 0 0 10px;width:38px;"">
						  <a href="""" style=""color:#ffffff;""><img src=""https://assets.codepen.io/210284/tw_1.png"" alt=""Twitter"" width=""38"" style=""height:auto;display:block;border:0;"" /></a>
						</td>
						<td style=""padding:0 0 0 10px;width:38px;"">
						  <a href="""" style=""color:#ffffff;""><img src=""https://assets.codepen.io/210284/fb_1.png"" alt=""Facebook"" width=""38"" style=""height:auto;display:block;border:0;"" /></a>
						</td>
					  </tr>
					</table>
				  </td>
				</tr>
			  </table>
			</td>
		  </tr>
		  
		</table>
	  </td>
	</tr>
  </table>
</body>
</html>
		");
		
		return sb.ToString();
	}
}
