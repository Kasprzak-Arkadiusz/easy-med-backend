using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class OfficeLocationConfiguration : IEntityTypeConfiguration<OfficeLocation>
{
    public void Configure(EntityTypeBuilder<OfficeLocation> builder)
    {
        builder.Property(ol => ol.Address)
            .HasMaxLength(109)
            .IsRequired();

        builder.HasOne(ol => ol.Doctor)
            .WithOne(d => d.OfficeLocation)
            .HasForeignKey<OfficeLocation>(of => of.DoctorId)
            .IsRequired(false);
    }
}