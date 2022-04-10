using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.Property(m => m.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(m => m.Capacity)
            .HasMaxLength(40)
            .IsRequired();
        builder.HasOne(m => m.Prescription)
            .WithMany(p => p.Medicines);
    }
}