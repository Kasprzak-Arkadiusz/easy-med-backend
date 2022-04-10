using EasyMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : UserConfiguration<Patient>
{
    public override void Configure(EntityTypeBuilder<Patient> builder)
    {
        base.Configure(builder);
        
        builder.Property(p => p.PersonalIdentityNumber)
            .HasColumnType("char")
            .HasMaxLength(11);
    }
}