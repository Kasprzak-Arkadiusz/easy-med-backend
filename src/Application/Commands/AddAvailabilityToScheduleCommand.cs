using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class AddAvailabilityToScheduleCommand : IRequest<IEnumerable<ScheduleViewModel>>
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int DoctorId { get; }
    public int CurrentUserId { get; }

    public AddAvailabilityToScheduleCommand(DateTime startDate, DateTime endDate, int doctorId, int currentUserId)
    {
        StartDate = startDate;
        EndDate = endDate;
        DoctorId = doctorId;
        CurrentUserId = currentUserId;
    }
}

public class AddAvailabilityToScheduleCommandHandler : IRequestHandler<AddAvailabilityToScheduleCommand, IEnumerable<ScheduleViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AddAvailabilityToScheduleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleViewModel>> Handle(AddAvailabilityToScheduleCommand query,
        CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == query.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        AuthorizationService.VerifyIfSameUser(query.DoctorId, query.CurrentUserId,
            "You can update only your schedule");

        if (query.StartDate < DateTime.Now || query.EndDate < DateTime.Now)
        {
            throw new BadRequestException("Both start date and end date must be in the future");
        }

        if (query.StartDate >= query.EndDate)
        {
            throw new BadRequestException("End date must be after start date");
        }

        var doesOverlapped = await _context.Schedules
            .AnyAsync(s => query.StartDate < s.EndDate && s.StartDate < query.EndDate,
                cancellationToken);

        if (doesOverlapped)
        {
            throw new BadRequestException("Some availabilities in schedule overlapped");
        }

        var schedule = Schedule.Create(query.StartDate, query.EndDate, doctor);
        await _context.Schedules.AddAsync(schedule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var schedules = await _context.Schedules
            .Where(s => s.Doctor.Id == query.DoctorId)
            .OrderByDescending(s => s.StartDate)
            .Select(s => _mapper.Map<ScheduleViewModel>(s))
            .ToListAsync(cancellationToken);

        return schedules;
    }
}