using System.Text;
using EasyMed.Domain.Common;
using EasyMed.Domain.Exceptions;

namespace EasyMed.Domain.Entities;

public class OfficeLocation : IEntity
{
    private const string AddressSeparator = ", ";

    public int Id { get; private set; }
    public string Street { get; private set; }
    public string House { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }

    public static OfficeLocation Create(string street, string house, string city, string postalCode, Doctor doctor)
    {
        return new OfficeLocation
        {
            Street = street,
            House = house,
            City = city,
            PostalCode = postalCode,
            Doctor = doctor
        };
    }

    public void Update(string street, string house, string city, string postalCode)
    {
        if (!string.IsNullOrEmpty(street) && Street != street)
        {
            Street = street;
        }

        if (!string.IsNullOrEmpty(house) && House != house)
        {
            House = house;
        }

        if (!string.IsNullOrEmpty(city) && City != city)
        {
            City = city;
        }

        if (!string.IsNullOrEmpty(postalCode) && PostalCode != postalCode)
        {
            PostalCode = postalCode;
        }
    }

    public static (string street, string house, string city, string postalCode) FullAddressToSeparateStrings(
        string fullAddress)
    {
        var dividedAddresses = fullAddress.Split(", ");
        if (dividedAddresses.Length != 4)
        {
            throw new MissingAddressDetailsException(
                "Address should have defined street, house number, city and postal code");
        }

        return (dividedAddresses[0], dividedAddresses[1], dividedAddresses[2], dividedAddresses[3]);
    }

    public string GetFullAddress()
    {
        var builder = new StringBuilder(120);
        builder.Append(Street);
        builder.Append(AddressSeparator);
        builder.Append(House);
        builder.Append(AddressSeparator);
        builder.Append(PostalCode);
        builder.Append(AddressSeparator);
        builder.Append(City);

        return builder.ToString();
    }
}