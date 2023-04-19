using System.Text;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
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
	
	public async Task<PagedList<MenuDto>> GetMenusPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken)
	{
		var entities = await this._menusRepository.GetPageAsync(pageNumber, pageSize, x => x.CreatedById == userId && !x.IsDeleted, cancellationToken);
		var dtos = this._mapper.Map<List<MenuDto>>(entities);
		var count = await this._menusRepository.GetTotalCountAsync(x => x.CreatedById == userId && !x.IsDeleted);
		
		return new PagedList<MenuDto>(dtos, pageNumber, pageSize, count);
	}
	
	public async Task<MenuDto> GetMenuAsync(ObjectId id, CancellationToken cancellationToken)
	{
		var entity = await this._menusRepository.GetMenuAsync(id, cancellationToken);
		return this._mapper.Map<MenuDto>(entity);
	}
	
	public async Task AddMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Menu>(dto);
		await this._menusRepository.AddAsync(entity, cancellationToken);
	}
	
	public async Task UpdateMenuAsync(MenuCreateDto dto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<Menu>(dto);
		await this._menusRepository.UpdateMenuAsync(entity, cancellationToken);
	}
	
	public async Task<bool> SendMenuToEmailAsync(ObjectId menuId, List<string> emailsTo, CancellationToken cancellationToken)
	{
		var entity = await this._menusRepository.GetMenuAsync(menuId, cancellationToken);
		var dto = this._mapper.Map<MenuDto>(entity);
		
		var message = new EmailMessage
		{
			Recipients = emailsTo,
			Subject = "Your Menus",
			Body = FormMenuEmailHTMLBodyAsync(dto).Result
		};
		
		try
		{
			await _emailsService.SendEmailMessageAsync(message, cancellationToken);
			return true;
		}
		catch(EmailSendException ex)
		{
			return false;
		}
	}
	
	private async Task<string> FormMenuEmailHTMLBodyAsync(MenuDto menu)
	{
		StringBuilder sb = new StringBuilder();
		foreach (var recipe in menu.Recipes)
		{
			sb.Append(recipe.Name + "\n");
		}
		return sb.ToString();
	} 
}
