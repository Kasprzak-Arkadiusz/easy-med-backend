using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class OfficeLocation : IEntity
{
    public int Id { get; private set; }
    public string Street { get; private set; }
    public string House { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    
    public Doctor Doctor { get; private set; }

    private OfficeLocation(string street, string house, string city, string postalCode, Doctor doctor)
    {
        Street = street;
        House = house;
        City = city;
        PostalCode = postalCode;
        Doctor = doctor;
    }

    public static OfficeLocation Create(string street, string house, string city, string postalCode, Doctor doctor)
    {
        return new OfficeLocation(street, house, city, postalCode, doctor);
    }

    public void Update(string street, string house, string city, string postalCode)
    {
        Street = street;
        House = house;
        City = city;
        PostalCode = postalCode;
    }
}