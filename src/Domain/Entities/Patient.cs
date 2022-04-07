using EasyMed.Domain.Enums;

namespace EasyMed.Domain.Entities;

public class Patient : User
{
    public string? PersonalIdentityNumber { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Visit> Visits { get; private set; }

    public static Patient Create(string emailAddress, string passwordHash)
    {
        return new Patient
        {
            EmailAddress = emailAddress,
            PasswordHash = passwordHash,
            Role = Role.Patient
        };
    }

    public void UpdatePersonalInformation(string firstName, string lastName, string emailAddress,
        string telephoneNumber, string personalIdentityNumber)
    {
        base.UpdatePersonalInformation(firstName, lastName, emailAddress, telephoneNumber);
        PersonalIdentityNumber = personalIdentityNumber;
    }
}