﻿using MongoDB.Bson;

namespace RecipesManagerApi.Application.Models.Operations;

public class OperationDetails
{
    public bool IsSuccessful { get; set; }

    public DateTime TimestampUtc { get; set; }
}
