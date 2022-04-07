using System.Reflection;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace EasyMed.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<OfficeLocation> OfficeLocations { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Visit> Visits { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

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