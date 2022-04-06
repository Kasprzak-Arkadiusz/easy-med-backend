using EasyMed.Application.Common.Interfaces;
using EasyMed.Infrastructure.Identity;
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
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(settings.DbConnectionString);
            options.EnableDetailedErrors();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}