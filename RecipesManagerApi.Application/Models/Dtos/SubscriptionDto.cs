using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models.Dtos;

public class SubscriptionDto
{
    public string Id { get; set; }

    public string? AuthorId { get; set; }

    public string? CreatedById { get; set; }

    public bool? IsAccessFull { get; set; }
}

