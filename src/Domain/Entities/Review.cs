using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Review : IEntity
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public short Rating { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    
    public static Review Create(string description, short rating, Doctor doctor, Patient patient)
    {
        return new Review
        {
            CreatedAt = DateTime.Now,
            Description = description,
            Rating = rating,
            Doctor = doctor,
            Patient = patient
        };
    }
}