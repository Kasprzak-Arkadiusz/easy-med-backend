using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Review : IEntity
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public short Rating { get; private set; }
    public DateOnly Date { get; private set; }
    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }


    public static Review Create(string description, short rating, DateOnly date, Doctor doctor, Patient patient)
    {
        return new Review
        {
            Description = description,
            Rating = rating,
            Date = date,
            Doctor = doctor,
            Patient = patient
        };
    }
}