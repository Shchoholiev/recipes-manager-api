using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class ShoppingList : EntityBase
{
	public String? Name { get; set; }
	
	public List<Ingredient>? Ingredients { get; set; }
	
	public List<ObjectId>? RecipesIds { get; set; }
	
	public String? Notes { get; set; }
	
	public List<Contact>? SentTo { get; set; }
	
	public bool IsDeleted { get; set; }
	
	public ObjectId? LastModifiedById { get; set; }

    public DateTime? LastModifiedDateUtc { get; set; } 
}