using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class OfficeLocationConfiguration : IEntityTypeConfiguration<OfficeLocation>
{
    public void Configure(EntityTypeBuilder<OfficeLocation> builder)
    {
        builder.Property(ol => ol.Street)
            .HasMaxLength(40)
            .IsRequired();
        builder.Property(ol => ol.House)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(ol => ol.City)
            .HasMaxLength(40)
            .IsRequired();
        builder.Property(ol => ol.PostalCode)
            .HasColumnType("char")
            .HasMaxLength(6)
            .IsRequired();
    }
}