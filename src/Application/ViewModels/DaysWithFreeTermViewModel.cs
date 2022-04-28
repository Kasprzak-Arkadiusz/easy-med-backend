﻿using DayOfWeek = EasyMed.Domain.Enums.DayOfWeek;

namespace EasyMed.Application.ViewModels;

public class DaysWithFreeTermViewModel
{
    public DateOnly Day { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}