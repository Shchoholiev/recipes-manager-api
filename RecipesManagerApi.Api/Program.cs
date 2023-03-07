using RecipesManagerApi.Infrastructure;
using RecipesManagerApi.Infrastructure.Database;
using RecipesManagerApi.Infrastructure.DataInitializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructure();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // var scope = app.Services.CreateScope();
    // var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    // var initializer = new MongoDbInitializer();
    // await initializer.Initialize(context);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
