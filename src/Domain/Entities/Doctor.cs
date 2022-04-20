using EasyMed.Domain.Enums;
using static BCrypt.Net.BCrypt;

namespace EasyMed.Domain.Entities;

public class Doctor : User
{
    public string Description { get; private set; }
    public string MedicalSpecialization { get; private set; }
    public OfficeLocation? OfficeLocation { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Schedule> Schedules { get; private set; }
    public ICollection<Visit> Visits { get; private set; }

    public static Doctor Create(string firstName, string lastName, string emailAddress, string password)
    {
        return new Doctor
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            PasswordHash = HashPassword(password),
            Role = Role.Doctor
        };
    }

    public void UpdatePersonalInformation(string firstName, string lastName, string telephoneNumber,
        string description, string? emailAddress = null)
    {
        base.UpdatePersonalInformation(firstName, lastName, emailAddress, telephoneNumber);
        Description = description;
    }

    public void ChangeMedicalSpecialization(MedicalSpecialization specialization)
    {
        MedicalSpecialization = specialization.ToString();
    }

    public override string GetFullName() => $"dr. {FirstName} {LastName}";

    public void ChangeOfficeLocation(OfficeLocation officeLocation)
    {
        OfficeLocation = officeLocation;
    }
}