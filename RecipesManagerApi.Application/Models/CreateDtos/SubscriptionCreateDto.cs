using System;

namespace RecipesManagerApi.Application.Models.CreateDtos;

public class SubscriptionCreateDto
{
    public string AuthorId { get; set; }

    public bool IsAccessFull { get; set; }
}

