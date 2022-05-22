using EasyMed.Domain.Enums;
using static BCrypt.Net.BCrypt;

namespace EasyMed.Domain.Entities;

public class Doctor : User
{
    public string Description { get; private set; }
    public string? MedicalSpecialization { get; private set; }
    public OfficeLocation? OfficeLocation { get; private set; }
    public ICollection<Prescription> Prescriptions { get; private set; }
    public ICollection<Review> Reviews { get; private set; }
    public ICollection<Schedule> Schedules { get; private set; }
    public ICollection<Visit> Visits { get; private set; }

    public static Doctor Create(string firstName, string lastName, string emailAddress, string password)
    {
        return new Doctor
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            PasswordHash = HashPassword(password),
            Role = Role.Doctor,
            Prescriptions = new List<Prescription>(),
            Reviews = new List<Review>(),
            Schedules = new List<Schedule>(),
            Visits = new List<Visit>()
        };
    }

    public void FillEntireScheduleByWeekSchedule(IReadOnlyCollection<Schedule> weekSchedules,
        int daysAhead = Schedule.DaysPlannedAhead)
    {
        var earliestScheduleDate = weekSchedules.OrderBy(s => s.StartDate).First().StartDate;
        int numberOfWeeksInThisPeriod = daysAhead / 7;
        var entireSchedule = new List<Schedule>();

        foreach (var schedule in weekSchedules)
        {
            entireSchedule.Add(schedule);

            for (int i = 1; i < numberOfWeeksInThisPeriod; i++)
            {
                entireSchedule.Add(Schedule.Create(schedule.StartDate.AddDays(i * 7),
                    schedule.EndDate.AddDays(i * 7), this));
            }

            var lastDay = earliestScheduleDate.AddDays(Schedule.DaysPlannedAhead);
            var scheduleLastDay = schedule.StartDate.AddDays(numberOfWeeksInThisPeriod * 7);

            if (scheduleLastDay <= lastDay)
            {
                entireSchedule.Add(Schedule.Create(scheduleLastDay, 
                    schedule.EndDate.AddDays(numberOfWeeksInThisPeriod * 7), this));
            }
        }

        Schedules = entireSchedule; 
    }

    public void UpdatePersonalInformation(string firstName, string lastName, string telephoneNumber,
        string description, string? emailAddress = null)
    {
        base.UpdatePersonalInformation(firstName, lastName, emailAddress, telephoneNumber);

        if (!string.IsNullOrEmpty(description) && Description != description)
        {
            Description = description;
        }
    }

    public void ChangeMedicalSpecialization(MedicalSpecialization? specialization)
    {
        if (specialization is not null && MedicalSpecialization != specialization.ToString())
        {
            MedicalSpecialization = specialization.ToString();
        }
    }

    public override string GetFullName() => $"{FirstName} {LastName}";

    public void UpdateOfficeLocation(string fullAddress)
    {
        if (OfficeLocation is null)
        {
            OfficeLocation = OfficeLocation.Create(fullAddress, this);
        }
        else
        {
            OfficeLocation.Update(fullAddress);
        }
    }
}