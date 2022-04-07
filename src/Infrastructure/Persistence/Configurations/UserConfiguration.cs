using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public abstract class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(40);
        builder.Property(u => u.LastName)
            .HasMaxLength(40);
        builder.Property(u => u.EmailAddress)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.TelephoneNumber)
            .HasColumnType("char")
            .HasMaxLength(9);
    }
}