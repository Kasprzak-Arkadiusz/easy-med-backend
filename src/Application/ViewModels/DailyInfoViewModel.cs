namespace EasyMed.Application.ViewModels;

public class DailyInfoViewModel
{
    public int RemainingVisits { get; set; }
    public DateTime? EndOfWorkAt { get; set; }
    public int IssuedPrescriptions { get; set; }
    public int? CurrentRating { get; set; }
}