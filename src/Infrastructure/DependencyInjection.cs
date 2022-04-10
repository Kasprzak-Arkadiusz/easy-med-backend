using EasyMed.Application.Common.Interfaces;
using EasyMed.Infrastructure.Persistence;
using EasyMed.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyMed.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        InfrastructureSettings settings)
    {
        services.AddSingleton(settings);
            
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseNpgsql(settings.DbConnectionString);
        });

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}