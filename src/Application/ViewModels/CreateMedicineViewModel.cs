namespace EasyMed.Application.ViewModels;

public class CreateMedicineViewModel
{
    public string Name { get; set; }
    public string Capacity { get; set; }

    public CreateMedicineViewModel(string name, string capacity)
    {
        Name = name;
        Capacity = capacity;
    }
}