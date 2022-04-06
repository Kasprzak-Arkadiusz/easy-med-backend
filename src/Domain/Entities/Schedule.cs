using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Schedule : IEntity
{
    public int Id { get; private set; }
    public string DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public Doctor Doctor { get; private set; }

    private Schedule(string dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Schedule Create(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        return new Schedule(dayOfWeek.ToString(), startTime, endTime);
    }
}