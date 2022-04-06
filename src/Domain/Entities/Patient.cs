namespace EasyMed.Domain.Entities;

public class Patient : User
{
    public string Pesel { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Visit> Visits { get; private set; }
    
    private Patient(string firstName, string lastName, string emailAddress, string telephoneNumber, string pesel) 
        : base(firstName, lastName, emailAddress, telephoneNumber)
    {
        Pesel = pesel;
    }

    public static Patient Create(string firstName, string lastName, string emailAddress, string telephoneNumber, string pesel)
    {
        return new Patient(firstName, lastName, emailAddress, telephoneNumber, pesel);
    }
}