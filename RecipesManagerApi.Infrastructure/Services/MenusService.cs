using System.Text;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.EmailModels;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class MenusService : IMenusService
{
	private readonly IMenusRepository _menusRepository;
	private readonly IRecipesRepository _recipesRepository;
	private readonly IMapper _mapper;
	private readonly IEmailsService _emailsService;
	
	public MenusService(
		IMenusRepository menusRepository,
		IRecipesRepository recipesRepository, 
		IMapper mapper,
		IEmailsService emailService)
	{
		this._menusRepository = menusRepository;
		this._mapper = mapper;
		this._emailsService = emailService;
		this._recipesRepository = recipesRepository;
	}
	
	public async Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, string userId, CancellationToken cancellationToken)
	{
		ObjectId.TryParse(userId, out var objectId);
		
		var entities = await this._menusRepository.GetPageAsync(pageNumber, pageSize, x => x.CreatedById == objectId, cancellationToken);
		var dtos = new List<MenuDto>(); 
		
		foreach(var menu in entities)
		{
			if (menu.RecipesIds == null)
			{
				dtos.Add(this._mapper.Map<MenuDto>(menu));
			}
			else
			{
				var recipesEntities = this._recipesRepository.GetPageAsync(1, 0, x => menu.RecipesIds.Contains(x.Id), cancellationToken);
				dtos.Add(this._mapper.Map<MenuDto>(menu, opt => opt.Items["Recipes"] = _mapper.Map<List<RecipeDto>>(recipesEntities)));
			}
		}
		var count = await this._menusRepository.GetTotalCountAsync(x => x.CreatedById == objectId && !x.IsDeleted);
		return new PagedList<MenuDto>(dtos, pageNumber, pageSize, count);
	}
	
	public async Task<MenuDto> GetMenuAsync(string id, CancellationToken cancellationToken)
	{
		ObjectId.TryParse(id, out var objectId);
		
		var menuEntity = await this._menusRepository.GetMenuAsync(objectId, cancellationToken);
		MenuDto dto;
		if (menuEntity.RecipesIds == null)
		{
			dto = this._mapper.Map<MenuDto>(menuEntity);
		}
		else
		{
			var recipesEntities = await this._recipesRepository.GetPageAsync(1, 0, x => menuEntity.RecipesIds.Contains(x.Id), cancellationToken);
			dto = this._mapper.Map<MenuDto>(menuEntity, opt => opt.Items["Recipes"] = this._mapper.Map<List<RecipeDto>>(recipesEntities));
		}
		return dto;	
	}
	
	public async Task<MenuDto> AddMenuAsync(MenuCreateDto createDto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Menu>(createDto);
		var newEntity = await this._menusRepository.AddAsync(entity, cancellationToken);
		
	
		if (newEntity.RecipesIds == null)
		{
			return this._mapper.Map<MenuDto>(entity);
		}
		else
		{
			var recipesEntities = await this._recipesRepository.GetPageAsync(1, 0, x => newEntity.RecipesIds.Contains(x.Id), cancellationToken);
			return this._mapper.Map<MenuDto>(entity, opt => opt.Items["Recipes"] = this._mapper.Map<List<RecipeDto>>(recipesEntities));
		}
	}
	
	public async Task<MenuDto> UpdateMenuAsync(MenuDto dto, CancellationToken cancellationToken)
	{
		Menu entity;
		if (dto.Recipes != null)
		{
			var recipesIds = new List<ObjectId>();
			foreach (var recipe in dto.Recipes)
			{
				recipesIds.Add(ObjectId.Parse(recipe.Id));
			}
			entity = this._mapper.Map<Menu>(dto, opt => opt.Items["RecipesIds"] = recipesIds);
		}
		else
		{
			entity = this._mapper.Map<Menu>(dto);
		}
		
		await this._menusRepository.UpdateMenuAsync(entity, cancellationToken);
		
		var recipesEntities = await this._recipesRepository.GetPageAsync(1, 0, x => entity.RecipesIds.Contains(x.Id), cancellationToken);
		return this._mapper.Map<MenuDto>(entity, opt => opt.Items["Recipes"] = this._mapper.Map<List<RecipeDto>>(recipesEntities));
	}
	
	public async Task SendMenuToEmailAsync(string menuId, List<string> emailsTo, CancellationToken cancellationToken)
	{
		var menuDto = await this.GetMenuAsync(menuId, cancellationToken);
		
		var message = new EmailMessage
		{
			Recipients = emailsTo,
			Subject = $"Menu {menuDto.Name}",
			Body = FormMenuEmailHTMLBody(menuDto)
		};
		
		await _emailsService.SendEmailMessageAsync(message, cancellationToken);
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
		
		if(menu.Recipes != null)
		{
			foreach (var recipe in menu.Recipes)
			{
				sb.Append(@$"
				<tr>
				<td>
				<p style=""margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;"">Ingredients for {recipe.Name}({recipe.ServingsCount} serving(s)):</p>
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
					<td><p>This menu contains no recipes</p></td>
				</tr>
			");
		}
		
		sb.Append(@"
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
