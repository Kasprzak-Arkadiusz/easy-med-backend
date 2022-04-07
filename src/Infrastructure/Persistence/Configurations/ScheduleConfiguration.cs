using EasyMed.Domain.Entities;
using EasyMed.Infrastructure.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyMed.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.Property(s => s.DayOfWeek)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(s => s.StartTime)
            .HasColumnType("time")
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>()
            .IsRequired();
        builder.Property(s => s.EndTime)
            .HasColumnType("time")
            .HasConversion<TimeOnlyConverter, TimeOnlyComparer>()
            .IsRequired();

        builder.HasOne(s => s.Doctor)
            .WithMany(d => d.Schedules);
    }
}