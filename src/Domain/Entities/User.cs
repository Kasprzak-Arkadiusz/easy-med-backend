using EasyMed.Domain.Common;
using EasyMed.Domain.Enums;

namespace EasyMed.Domain.Entities;

public abstract class User : IEntity
{
    public int Id { get; protected init; }
    public string PasswordHash { get; protected init; }
    public Role Role { get; protected init; }
    public string EmailAddress { get; protected set; }
    public string? FirstName { get; protected set; }
    public string? LastName { get; protected set; }
    public string? TelephoneNumber { get; protected set; }

    protected void UpdatePersonalInformation(string firstName, string lastName, string emailAddress,
        string telephoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        TelephoneNumber = telephoneNumber;
    }
}