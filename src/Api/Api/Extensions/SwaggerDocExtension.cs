using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class SwaggerDocExtension
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Easy Med API",
                    Description = "An ASP.NET Web API for Easy Med Application",
                });
            
            opt.SupportNonNullableReferenceTypes();
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}