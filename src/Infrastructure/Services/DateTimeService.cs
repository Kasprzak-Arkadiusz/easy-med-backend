using easy_med_backend.Application.Common.Interfaces;

namespace easy_med_backend.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}