using System.Text;
using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.GlodalInstances;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.EmailModels;
using RecipesManagerApi.Application.Models.Operations;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Infrastructure.Services;

public class ShoppingListsService : IShoppingListsService
{
	private readonly IMapper _mapper;
	private readonly IShoppingListsRepository _shoppingListsRepository;
	private readonly IEmailsService _emailsService;
	
	public ShoppingListsService(IMapper mapper, IShoppingListsRepository shoppingListsRepository, IEmailsService emailsService)
	{
		this._mapper = mapper;
		this._shoppingListsRepository = shoppingListsRepository;
		this._emailsService = emailsService;
	}
	
	public async Task<PagedList<ShoppingListDto>> GetShoppingListsPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
	{
		var userId = GlobalUser.Id.Value;
		var entities = await this._shoppingListsRepository.GetPageAsync(pageNumber, pageSize, userId, cancellationToken);
		var dtos = this._mapper.Map<List<ShoppingListDto>>(entities);
		var count = await this._shoppingListsRepository.GetTotalCountAsync(x => !x.IsDeleted && x.CreatedById == userId);
		return new PagedList<ShoppingListDto>(dtos, pageNumber, pageSize, count);
	}

	public async Task<ShoppingListDto> AddShoppingListAsync(ShoppingListCreateDto shoppingListDto, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<ShoppingList>(shoppingListDto);
		entity.CreatedById = GlobalUser.Id.Value;
		entity.CreatedDateUtc = DateTime.UtcNow;
		var newEntity = await this._shoppingListsRepository.AddShoppingListAsync(entity, cancellationToken);
		return this._mapper.Map<ShoppingListDto>(newEntity);
	}

	public async Task<OperationDetails> DeleteShoppingListAsync(string id, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(id, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var entity = await this._shoppingListsRepository.GetShoppingListAsync(objectId, cancellationToken);
		entity.IsDeleted = true;
		await this._shoppingListsRepository.UpdateShoppingListAsync(entity, cancellationToken);
		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}

	public async Task<ShoppingListDto> GetShoppingListAsync(string id, CancellationToken cancellationToken)
	{
		if (!ObjectId.TryParse(id, out var objectId))
		{
			throw new InvalidDataException("Provided id is invalid.");
		}
		var entity = await this._shoppingListsRepository.GetShoppingListLookedUpAsync(objectId, cancellationToken);
		if(entity == null)
		{
			throw new EntityNotFoundException<ShoppingList>();
		}
		return this._mapper.Map<ShoppingListDto>(entity);
	}

	public async Task<ShoppingListDto> UpdateShoppingListAsync(ShoppingListCreateDto shoppingList, CancellationToken cancellationToken)
	{
		var entity = this._mapper.Map<ShoppingList>(shoppingList);
		var entityLookedUp = await this._shoppingListsRepository.UpdateShoppingListAsync(entity, cancellationToken);
		var dto = this._mapper.Map<ShoppingListDto>(entityLookedUp);
		return dto;
	}
	
	public async Task<OperationDetails> SendShoppingListToEmailsAsync(string id, IEnumerable<string> emailsTo, CancellationToken cancellationToken)
	{
		var shoppingListDto = await this.GetShoppingListAsync(id, cancellationToken);
		var message = new EmailMessage
		{
			Recipients = emailsTo.ToList(),
			Subject = "Shopping list",
			Body = FormEmailHTMLBody(shoppingListDto)
		};
		await _emailsService.SendEmailMessageAsync(message, cancellationToken);
		return new OperationDetails { IsSuccessful = true, TimestampUtc = DateTime.UtcNow };
	}
	
	private string FormEmailHTMLBody(ShoppingListDto shoppingList)
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
					<h1 style=""font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;text-align:center"">Shopping List</h1>
				  </td>
				</tr>
		");
		
		if(shoppingList.Recipes.Count > 0)
		{
			foreach (var recipe in shoppingList.Recipes)
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
					<td><h1 style=""color:#153643;font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;text-align:center"">Shopping list is empty</h1></td>
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