using EasyMed.Domain.Entities;
using EasyMed.Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Description)
            .HasMaxLength(400)
            .IsRequired(false);
        builder.Property(r => r.Rating)
            .HasColumnType("smallint")
            .IsRequired();
        builder.Property(r => r.CreatedAt)
            .IsRequired();
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Patient)
            .WithMany(pt => pt.Reviews)
            .HasForeignKey(r => r.PatientId);
        builder.HasOne(r => r.Doctor)
            .WithMany(d => d.Reviews)
            .HasForeignKey(r => r.DoctorId);
    }
}