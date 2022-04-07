using System.Text.Json.Serialization;
using Api.Extensions;
using EasyMed.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

var infrastructureSettings = new InfrastructureSettings();
configuration.Bind(nameof(InfrastructureSettings), infrastructureSettings);
builder.Services.AddInfrastructure(infrastructureSettings);

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

app.UseAuthorization();
app.MapControllers();

app.Run();