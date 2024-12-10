using System.Reflection;
using TFA.API.DI;
using TFA.API.Middlewares;
using TFA.Application.DI;
using TFA.Storage.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogging(builder.Configuration, builder.Environment);

string? connectionString = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddForumDomain()
                .AddForumStorage(connectionString);

builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program { }