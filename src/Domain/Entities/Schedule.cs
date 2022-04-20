using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Schedule : IEntity
{
    public int Id { get; private set; }
    public string DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public Doctor Doctor { get; private set; }

    public static Schedule Create(Enums.DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, Doctor doctor)
    {
        return new Schedule
        {
            DayOfWeek = dayOfWeek.ToString(), 
            StartTime = startTime, 
            EndTime = endTime, 
            Doctor = doctor
        };
    }

    public void ChangeSchedule(Enums.DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        DayOfWeek = dayOfWeek.ToString();
        StartTime = startTime;
        EndTime = endTime;
    }
}