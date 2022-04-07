using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Prescription : IEntity
{
    public int Id { get; private set; }
    public DateOnly DateOfIssue { get; private set; }
    public Patient Patient { get; private set; }
    public int PatientId { get; private set; }
    public Doctor Doctor { get; private set; }
    public int DoctorId { get; private set; }
    public ICollection<Medicine> Medicines { get; private set; }

    public static Prescription Create(DateOnly dateOfIssue, Doctor doctor, Patient patient, ICollection<Medicine> medicines)
    {
        return new Prescription
        {
            DateOfIssue = dateOfIssue, 
            Doctor = doctor, 
            Patient = patient, 
            Medicines = medicines
        };
    }
}