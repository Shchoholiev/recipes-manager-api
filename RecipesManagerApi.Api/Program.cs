using RecipesManagerApi.Api;
using RecipesManagerApi.Api.CustomMiddlewares;
using RecipesManagerApi.Infrastructure;
using RecipesManagerApi.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

var appConfig = Environment.GetEnvironmentVariable("APP_CONFIG") ?? builder.Configuration.GetConnectionString("AppConfig");
builder.Configuration.AddAzureAppConfiguration(appConfig);

builder.Services.AddJWTTokenAuthentication(builder.Configuration);
builder.Services.AddHttpClients(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddMapper();
builder.Services.AddServices();
builder.Services.AddGraphQl();
builder.Services.ConfigureCors();
builder.Services.AddControllers();
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddHttpContextAccessor();

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

app.MapHealthChecks("/health");

app.Run();