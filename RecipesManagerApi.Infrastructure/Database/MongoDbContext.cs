using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace RecipesManagerApi.Infrastructure.Database;

public class MongoDbContext
{
    private readonly MongoClient _client;

    private readonly IMongoDatabase _db;

    public MongoDbContext(IConfiguration configuration)
    {
        this._client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        this._db = this._client.GetDatabase(configuration.GetConnectionString("MongoDatabaseName"));
    }

    public IMongoDatabase Db => this._db; 
}