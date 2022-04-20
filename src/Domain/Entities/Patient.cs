using EasyMed.Domain.Enums;
using static BCrypt.Net.BCrypt;

namespace EasyMed.Domain.Entities;

public class Patient : User
{
    public string? PersonalIdentityNumber { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Visit> Visits { get; private set; }

    public static Patient Create(string firstName, string lastName, string emailAddress, string password)
    {
        return new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            PasswordHash =  HashPassword(password),
            Role = Role.Patient
        };
    }

    public void UpdatePersonalInformation(string firstName, string lastName,
        string telephoneNumber, string personalIdentityNumber, string? emailAddress = null)
    {
        base.UpdatePersonalInformation(firstName, lastName, emailAddress, telephoneNumber);
        PersonalIdentityNumber = personalIdentityNumber;
    }
}