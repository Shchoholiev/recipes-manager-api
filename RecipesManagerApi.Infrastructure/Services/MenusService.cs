using System.Text;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
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

namespace RecipesManagerApi.Infrastructure.Services;

public class MenusService : IMenusService
{
	private readonly IMenusRepository _menusRepository;
	private readonly IMapper _mapper;
	private readonly IEmailsService _emailsService;
	
	public MenusService(
		IMenusRepository menusRepository,
		IMapper mapper,
		IEmailsService emailService)
	{
		this._menusRepository = menusRepository;
		this._mapper = mapper;
		this._emailsService = emailService;
	}
	
	public async Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		ObjectId.TryParse(GlobalUser.Id.ToString(), out var userId);
		var entities = await this._menusRepository.GetPageAsync(pageNumber, pageSize, userId, cancellationToken);
		var dtos = this._mapper.Map<List<MenuDto>>(entities);
		var count = await this._menusRepository.GetTotalCountAsync(x => !x.IsDeleted && x.CreatedById == userId);
		
		return new PagedList<MenuDto>(dtos, pageNumber, pageSize, count);
	}
	
	public async Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken)
	{
		ObjectId.TryParse(id, out var objectId);
		var entity = await this._menusRepository.GetMenuAsync(objectId, cancellationToken);
		if(entity == null)
		{
			throw new EntityNotFoundException<Menu>();
		}
		return this._mapper.Map<MenuDto>(entity);
	}
	
	public async Task<MenuDto> AddMenuAsync(MenuCreateDto createDto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Menu>(createDto);
		var menu = await this._menusRepository.AddMenuAsync(entity, cancellationToken);
		
		return this._mapper.Map<MenuDto>(menu);
	}
	
	public async Task<MenuDto> UpdateMenuAsync(MenuDto dto, CancellationToken cancellationToken)
	{
		List<string> recipesIds = new List<string>();
		if(dto.Recipes != null)
		{
			recipesIds = dto.Recipes.Select(x => x.Id).ToList();
		}
		var entity = this._mapper.Map<Menu>(dto, opt => opt.Items["RecipesIds"] =  recipesIds);
		await this._menusRepository.UpdateMenuAsync(entity, cancellationToken);
		
		return dto;
	}
	
	public async Task<OperationDetails> DeleteMenuAsync(MenuDto dto, CancellationToken cancellationToken)
	{
		List<string> recipesIds = new List<string>();
		if(dto.Recipes != null)
		{
			recipesIds = dto.Recipes.Select(x => x.Id).ToList();
		}
		var entity = this._mapper.Map<Menu>(dto, opt => opt.Items["RecipesIds"] =  recipesIds);
		entity.IsDeleted = true;
		await this._menusRepository.UpdateMenuAsync(entity, cancellationToken);
		
		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}
	
	public async Task<OperationDetails> SendMenuToEmailAsync(string menuId, List<string> emailsTo, CancellationToken cancellationToken)
	{
		var menuDto = await this.GetMenuAsync(menuId, cancellationToken);
		var message = new EmailMessage
		{
			Recipients = emailsTo,
			Subject = $"Recipes for {menuDto.Name}:",
			Body = FormMenuEmailHTMLBody(menuDto)
		};
		await _emailsService.SendEmailMessageAsync(message, cancellationToken);
		
		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
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
		
		if(menu.Recipes.Count > 0)
		{
			foreach (var recipe in menu.Recipes)
			{
				sb.Append(@$"
				<tr>
				<td>
				<p style=""margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;"">Ingredients for {recipe.Name} for {recipe.ServingsCount} serving(s):</p>
				");
				
				foreach(var ingredient in recipe.Ingredients)
				{
					sb.Append($"<p>- {ingredient.Name}: {ingredient.Amount} {ingredient.Units}</p>");
				}
				
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
