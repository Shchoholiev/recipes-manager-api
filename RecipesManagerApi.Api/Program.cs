using RecipesManagerApi.Api;
using RecipesManagerApi.Api.CustomMiddlewares;
using RecipesManagerApi.Infrastructure;
using RecipesManagerApi.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenAuthentication(builder.Configuration);
builder.Services.AddHttpClients(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddMapper();
builder.Services.AddServices();
builder.Services.AddGraphQl();
builder.Services.ConfigureCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //var scope = app.Services.CreateScope();
    //var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    //var initializer = new MongoDbInitializer();
    //await initializer.Initialize(context);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("allowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.ConfogureGlobalUserMiddleware();

app.MapGraphQL();

app.MapControllers();

app.Run();