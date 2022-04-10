using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : UserConfiguration<Doctor>
{
    public override void Configure(EntityTypeBuilder<Doctor> builder)
    {
        base.Configure(builder);

        builder.Property(d => d.Description)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(d => d.MedicalSpecialization)
            .HasMaxLength(40)
            .IsRequired();
        builder.HasOne(d => d.OfficeLocation)
            .WithOne(ol => ol.Doctor)
            .HasForeignKey<Doctor>(d => d.OfficeLocationId)
            .IsRequired();
    }
}