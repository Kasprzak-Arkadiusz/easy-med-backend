using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Medicine : IEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Capacity { get; private set; }
    public Prescription Prescription { get; private set; }

    private Medicine(string name, string capacity)
    {
        Name = name;
        Capacity = capacity;
    }

    public static Medicine Create(string name, string capacity)
    {
        return new Medicine(name, capacity);
    }
    
}