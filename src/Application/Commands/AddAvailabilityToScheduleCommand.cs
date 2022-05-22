using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class Availability
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class AddAvailabilityToScheduleCommand : IRequest<IEnumerable<ScheduleViewModel>>
{
    public IEnumerable<Availability> Availabilities { get; }
    public int DoctorId { get; }
    public int CurrentUserId { get; }

    public AddAvailabilityToScheduleCommand(IEnumerable<Availability> availabilities, int doctorId, int currentUserId)
    {
        Availabilities = availabilities;
        DoctorId = doctorId;
        CurrentUserId = currentUserId;
    }
}

public class
    AddAvailabilityToScheduleCommandHandler : IRequestHandler<AddAvailabilityToScheduleCommand,
        IEnumerable<ScheduleViewModel>>
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

        await using var transaction = await ((DbContext)_context).Database.BeginTransactionAsync(cancellationToken);
        foreach (Availability availability in query.Availabilities)
        {
            if (availability.StartDate < DateTime.Now || availability.EndDate < DateTime.Now)
            {
                throw new BadRequestException("Both start date and end date must be in the future");
            }

            if (availability.StartDate >= availability.EndDate)
            {
                throw new BadRequestException("End date must be after start date");
            }

            var doesOverlapped = await _context.Schedules
                .Where(s => s.DoctorId == query.DoctorId)
                .AnyAsync(
                    s => availability.StartDate < s.EndDate && s.StartDate < availability.EndDate,
                    cancellationToken);

            if (doesOverlapped)
            {
                throw new BadRequestException("Some availabilities in schedule overlapped");
            }

            var schedule = Schedule.Create(availability.StartDate, availability.EndDate, doctor);
            await _context.Schedules.AddAsync(schedule, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        var schedules = await _context.Schedules
            .Where(s => s.Doctor.Id == query.DoctorId)
            .OrderByDescending(s => s.StartDate)
            .Select(s => _mapper.Map<ScheduleViewModel>(s))
            .ToListAsync(cancellationToken);

        return schedules;
    }
}