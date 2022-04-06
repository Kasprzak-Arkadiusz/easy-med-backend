using EasyMed.Domain.Enums;

namespace EasyMed.Domain.Entities;

public class Doctor : User
{
    public string Description { get; private set; }
    public string MedicalSpecialization { get; private set; }

    public int OfficeLocationId { get; private set; }
    public OfficeLocation OfficeLocation { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Schedule> Schedules { get; private set; }
    public ICollection<Visit> Visits { get; private set; }
    
    private Doctor(string firstName, string lastName, string emailAddress, string telephoneNumber, string description,
        string medicalSpecialization)
        : base(firstName, lastName, emailAddress, telephoneNumber)
    {
        Description = description;
        MedicalSpecialization = medicalSpecialization;
    }

    public static Doctor Create(string firstName, string lastName, string emailAddress, string telephoneNumber,
        string description, MedicalSpecialization specialization)
    {
        return new Doctor(firstName, lastName, emailAddress, telephoneNumber, description, specialization.ToString());
    }
}