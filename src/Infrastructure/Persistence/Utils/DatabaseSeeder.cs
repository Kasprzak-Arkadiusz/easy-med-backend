using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Entities;
using EasyMed.Domain.Enums;
using DayOfWeek = EasyMed.Domain.Enums.DayOfWeek;

namespace EasyMed.Infrastructure.Persistence.Utils;

public static class DatabaseSeeder
{
    public async static Task Seed(IApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        List<Doctor> doctors = await SeedDoctors(context);
        List<Patient> patients = await SeedPatients(context);

        await SeedOfficeLocations(context, doctors);
        await SeedSchedules(context, doctors);
        await SeedVisits(context, doctors, patients);
        await SeedReviews(context, doctors, patients);
        List<Medicine> medicines = await SeedMedicines(context);

        await SeedPrescriptions(context, doctors, patients, medicines);
        await context.SaveChangesAsync();
    }

    private static async Task<List<Doctor>> SeedDoctors(IApplicationDbContext applicationDbContext)
    {
        var list = new List<Doctor>
        {
            Doctor.Create("Filip", "Wiśniewski","bestDoctor@gmail.com", "rhinitiS72"),
            Doctor.Create("Konrad", "Zabrzecki","drKonradZabrzecki123@onet.pl", "12Influenza"),
            Doctor.Create("Elżbieta", "Nowak","ElzbietaNowakDr44@wp.pl", "tuSsiS123")
        };
        list[0].UpdatePersonalInformation("Filip", "Wiśniewski", "121212121",
            "My name is Doctor Filip. I am licensed health professional " +
            "who maintain and restore human health through the practice of medicine");
        list[1].UpdatePersonalInformation("Konrad", "Zabrzecki", "637482911","");
        list[2].UpdatePersonalInformation("Elżbieta", "Nowak", "648123392","");
        list[1].ChangeMedicalSpecialization(MedicalSpecialization.Geriatrician);
        await applicationDbContext.Doctors.AddRangeAsync(list);
        return list;
    }

    private static async Task<List<Patient>> SeedPatients(IApplicationDbContext context)
    {
        var patients = new List<Patient>
        {
            Patient.Create("Michał", "Nowakowski","MichalNowakowski6428@gmail.com", "Michal6428"),
            Patient.Create("Janek", "Kowalski","JohnDoe@gmail.com", "DoeJohn13")
        };
        patients[0].UpdatePersonalInformation("Michał", "Nowakowski", "664664664", "00081205417");
        await context.Patients.AddRangeAsync(patients);
        return patients;
    }

    private static async Task SeedOfficeLocations(IApplicationDbContext context, IReadOnlyList<Doctor> doctors)
    {
        var officeLocations = new List<OfficeLocation>
        {
            OfficeLocation.Create("ul. Generała Bema", "15", "Białystok", "15-458", doctors[0]),
            OfficeLocation.Create("al. Jerozolimskie", "34", "Warszawa", "05-800", doctors[1]),
            OfficeLocation.Create("ul. Marii Skłodowskiej-Curie", "24A", "Białystok", "15-276", doctors[2])
        };
        await context.OfficeLocations.AddRangeAsync(officeLocations);
    }

    private static async Task SeedSchedules(IApplicationDbContext context, IReadOnlyList<Doctor> doctors)
    {
        var schedules = Enum.GetValues<DayOfWeek>().Where(day => day != DayOfWeek.Sunday)
            .Select(day => Schedule.Create(day, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0), doctors[2])).ToList();

        schedules.AddRange(new[]
        {
            Schedule.Create(DayOfWeek.Monday, new TimeOnly(16, 0, 0), new TimeOnly(19, 30, 0), doctors[1]),
            Schedule.Create(DayOfWeek.Tuesday, new TimeOnly(12, 0, 0), new TimeOnly(18, 0, 0), doctors[1]),
            Schedule.Create(DayOfWeek.Thursday, new TimeOnly(12, 0, 0), new TimeOnly(18, 0, 0), doctors[1]),
            Schedule.Create(DayOfWeek.Saturday, new TimeOnly(14, 0, 0), new TimeOnly(20, 0, 0), doctors[1]),
            Schedule.Create(DayOfWeek.Sunday, new TimeOnly(9, 30, 0), new TimeOnly(16, 0, 0), doctors[1])
        });

        schedules.AddRange(new[]
        {
            Schedule.Create(DayOfWeek.Tuesday, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0), doctors[0]),
            Schedule.Create(DayOfWeek.Wednesday, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0), doctors[0]),
            Schedule.Create(DayOfWeek.Thursday, new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0), doctors[0])
        });
        await context.Schedules.AddRangeAsync(schedules);
    }

    private static async Task SeedVisits(IApplicationDbContext context, IReadOnlyList<Doctor> doctors,
        IReadOnlyList<Patient> patients)
    {
        var visits = new List<Visit>
        {
            Visit.Create(new DateTime(2022, 04, 27, 12, 30, 0), doctors[2], patients[0]),
            Visit.Create(new DateTime(2022, 05, 04, 15, 30, 0), doctors[2], patients[0]),
            Visit.Create(new DateTime(2022, 04, 25, 16, 00, 0), doctors[1], patients[0])
        };
        await context.Visits.AddRangeAsync(visits);
    }

    private static async Task SeedReviews(IApplicationDbContext context, IReadOnlyList<Doctor> doctors,
        IReadOnlyList<Patient> patients)
    {
        var reviews = new List<Review>
        {
            Review.Create(
                "Doktor Elżbieta Nowak – bardzo profesjonalna i miła Pani doktor, trafna diagnoza," +
                " przepisane leczenie szybko przyniosło ulgę. Dziękuje!",4, doctors[2], patients[0]),
            Review.Create(
                "Z czystym sumieniem polecam dr. Konrada. Pełen profesjonalizm, wiedza i siła spokoju " +
                "i umiejętność wytłumaczenia mi przebiegu choroby, spowodowały, że po 30 minutach rozmowy z p. doktorem " +
                "jestem spokojny o zdrowie moje mamy.", 5, doctors[1], patients[1])
        };
        await context.Reviews.AddRangeAsync(reviews);
    }

    private static async Task<List<Medicine>> SeedMedicines(IApplicationDbContext context)
    {
        var medicines = new List<Medicine>
        {
            Medicine.Create("Dymista aer.do nosa, zaw.", "(0,137mg+0,005mg)/daw."),
            Medicine.Create("Neozine Max tab.do ustne", "(0,25mg)/daw."),
            Medicine.Create("Cogiton ", "1 tabletka dziennie")
        };
        await context.Medicines.AddRangeAsync(medicines);
        return medicines;
    }

    private static async Task SeedPrescriptions(IApplicationDbContext context, IReadOnlyList<Doctor> doctors,
        IReadOnlyList<Patient> patients, IReadOnlyList<Medicine> medicines)
    {
        var prescriptions = new List<Prescription>
        {
            Prescription.Create(new DateOnly(2022, 04, 10), doctors[2], patients[0],
                new List<Medicine> { medicines[0], medicines[1] }),
            Prescription.Create(new DateOnly(2022, 04, 12), doctors[1], patients[1],
                new List<Medicine> { medicines[2] })
        };
        await context.Prescriptions.AddRangeAsync(prescriptions);
    }
}