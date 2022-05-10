namespace EasyMed.Application.ViewModels;

public class CreateMedicineViewModel
{
    public CreateMedicineViewModel(string name, string capacity)
    {
        Name = name;
        Capacity = capacity;
    }
    public string Name { get; set; }
    public string Capacity { get; set; }
}