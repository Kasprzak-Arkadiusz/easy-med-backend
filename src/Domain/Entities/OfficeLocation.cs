using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class OfficeLocation : IEntity
{
    public int Id { get; private set; }
    public string Address { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }

    public static OfficeLocation Create(string address, Doctor doctor)
    {
        return new OfficeLocation
        {
            Address = address,
            Doctor = doctor
        };
    }

    public void Update(string address)
    {
        if (!string.IsNullOrEmpty(address) && Address != address)
        {
            Address = address;
        }
    }
}