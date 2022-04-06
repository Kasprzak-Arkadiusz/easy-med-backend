using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public abstract class User : IEntity
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }
    public string TelephoneNumber { get; private set; }

    protected User(string firstName, string lastName, string emailAddress, string telephoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        TelephoneNumber = telephoneNumber;
    }
}