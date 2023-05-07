using RecipesManagerApi.Domain.Common;
using MongoDB.Bson;

namespace RecipesManagerApi.Domain.Entities;

public class Menu : EntityBase
{
	public string Name { get; set; }
	
	public List<ObjectId>? RecipesIds { get; set; }
	
	public string? Notes { get; set; }
	
	public List<ObjectId>? SentTo { get; set; }
	
	public bool IsDeleted { get; set; }
	
	public DateTime? ForDateUtc { get; set; } 
	
	public ObjectId? LastModifiedById { get; set; }
	
	public DateTime? LastModifiedDateUtc { get; set; }
}
