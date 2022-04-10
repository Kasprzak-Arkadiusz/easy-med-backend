﻿using EasyMed.Domain.Common;

namespace EasyMed.Domain.Entities;

public class Visit : IEntity
{
    public int Id { get; private set; }
    public DateTime DateTime { get; private set; }
    public bool IsCompleted { get; private set; }
    public Doctor Doctor { get; private set; }
    public int DoctorId { get; private set; }
    public Patient Patient { get; private set; }
    public int PatientId { get; private set; }

    public static Visit Create(DateTime dateTime, Doctor doctor, Patient patient)
    {
        return new Visit
        {
            DateTime = dateTime, 
            IsCompleted = false, 
            Doctor = doctor, 
            Patient = patient
        };
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}