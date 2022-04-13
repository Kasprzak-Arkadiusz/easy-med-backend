using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Doctor> Doctors { get; }
    public DbSet<Medicine> Medicines { get; }
    public DbSet<OfficeLocation> OfficeLocations { get; }
    public DbSet<Patient> Patients { get; }
    public DbSet<Prescription> Prescriptions { get; }
    public DbSet<Review> Reviews { get; }
    public DbSet<Schedule> Schedules { get; }
    public DbSet<User> Users { get;}
    public DbSet<Visit> Visits { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}