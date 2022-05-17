using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Schedule : IEntity
{
    public int Id { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Doctor Doctor { get; private set; }
    public int DoctorId { get; private set; }

    public const int DaysPlannedAhead = 90;

    public static Schedule Create(DateTime startDate, DateTime endDate, Doctor doctor)
    {
        return new Schedule
        {
            StartDate = startDate,
            EndDate = endDate,
            Doctor = doctor,
            DoctorId = doctor.Id
        };
    }
}