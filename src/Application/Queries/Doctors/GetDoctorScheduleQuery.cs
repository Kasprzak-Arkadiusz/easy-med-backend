using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetDoctorScheduleQuery : IRequest<IEnumerable<ScheduleViewModel>>
{
    public int CurrentUserId { get; }
    public int DoctorId { get; }

    public GetDoctorScheduleQuery(int currentUserId, int doctorId)
    {
        CurrentUserId = currentUserId;
        DoctorId = doctorId;
    }
}

public class GetDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, IEnumerable<ScheduleViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDoctorScheduleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleViewModel>> Handle(GetDoctorScheduleQuery query,
        CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == query.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        AuthorizationService.VerifyIfSameUser(query.DoctorId, query.CurrentUserId,
            "You cannot see a schedule that is not yours");

        var schedules = await _context.Schedules
            .Where(s => s.Doctor.Id == query.DoctorId)
            .OrderByDescending(s => s.StartDate)
            .Select(s => _mapper.Map<ScheduleViewModel>(s))
            .ToListAsync(cancellationToken);

        return schedules;
    }
}