using EasyMed.Domain.Common;
using EasyMed.Domain.Exceptions;

namespace EasyMed.Domain.Entities;

public class Visit : IEntity
{
    private const int VisitTimeInMinutes = 30;
    public int Id { get; private set; }
    public DateTime DateTime { get; private set; }
    public bool IsCompleted { get; private set; }
    public Doctor Doctor { get; private set; }
    public int DoctorId { get; private set; }
    public Patient Patient { get; private set; }
    public int PatientId { get; private set; }

    public static Visit Create(DateTime dateTime, Doctor doctor, Patient patient)
    {
        if (doctor.OfficeLocation is null)
        {
            throw new VisitWithoutLocationException("Cannot create a visit without doctor office location");
        }

        return new Visit { DateTime = dateTime, IsCompleted = false, Doctor = doctor, Patient = patient };
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public static int GetVisitTimeInMinutes() => VisitTimeInMinutes;
}