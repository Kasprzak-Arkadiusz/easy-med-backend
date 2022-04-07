using System.Reflection;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Doctor> Doctors { get; }
    public DbSet<Medicine> Medicines { get; }
    public DbSet<OfficeLocation> OfficeLocations { get; }
    public DbSet<Patient> Patients { get; }
    public DbSet<Prescription> Prescriptions { get; }
    public DbSet<Review> Reviews { get; }
    public DbSet<Schedule> Schedules { get; }
    public DbSet<User> Users { get; }
    public DbSet<Visit> Visits { get; }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}