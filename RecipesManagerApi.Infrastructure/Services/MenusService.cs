using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Domain.Entities;
using RecipesManagerApi.Infrastructure.Email;

namespace RecipesManagerApi.Infrastructure.Services;

public class MenusService : IMenusService
{
	private readonly IMenusRepository _menusRepository;
	private readonly IMapper _mapper;
	private readonly EmailService _emailService;
	
	public MenusService(IMenusRepository menusRepository, IMapper mapper, EmailService emailService)
	{
		this._menusRepository = menusRepository;
		this._mapper = mapper;
		this._emailService = emailService;
	}
	
	public async Task<List<Menu>> GetMenusPageAsync(int pageNumber, int pageSize, ObjectId userId, CancellationToken cancellationToken)
	{
		return await this._menusRepository.GetPageAsync(pageNumber, pageSize, x => x.CreatedById == userId , cancellationToken);
	}
	
	public async Task<Menu> GetMenuAsync(ObjectId id, CancellationToken cancellationToken)
	{
		var menu = await this._menusRepository.GetMenuAsync(id, cancellationToken);
		
	}
	
	public async Task CreateMenuAsync(ObjectId userId, List<ObjectId> recipesIds, CancellationToken cancellationToken)
	{
		var menu = new Menu
		{
			CreatedById = userId,
			RecipesIds = recipesIds
		};
		await this._menusRepository.AddAsync(menu, cancellationToken);
		
	}
	
	public async Task SendMailToMenuAsync(ObjectId menuId, List<string> emails, CancellationToken cancellationToken)
	{
		var menu = await this._menusRepository.GetMenuAsync(menuId, cancellationToken);
		var EmailMessage = new EmailMessage()
		{
			Recipients = emails,
			Subject = "",
			Body = ""	
		};
		
	} 
}
