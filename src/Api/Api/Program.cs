using System.Text.Json.Serialization;
using Api.Extensions;
using Api.Middlewares;
using EasyMed.Application;
using EasyMed.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

var infrastructureSettings = new InfrastructureSettings();
configuration.Bind(nameof(InfrastructureSettings), infrastructureSettings);
builder.Services.AddInfrastructure(infrastructureSettings);
builder.Services.AddApplication();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerDocumentation();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.UseCors(opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowAnyOrigin();
});

app.Run();