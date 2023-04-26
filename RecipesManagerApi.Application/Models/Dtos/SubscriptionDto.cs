using System;
using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models.Dtos;

public class SubscriptionDto
{
    public string Id { get; set; }

    public ObjectId AuthorId { get; set; }

}

