namespace EasyMed.Application.ViewModels;

public class DailyInfoViewModel
{
    public int RemainingVisits { get; set; }
    public string? EndOfWorkAt { get; set; }
    public int IssuedPrescriptions { get; set; }
    public int? CurrentRating { get; set; }
}