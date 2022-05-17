using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetDaysWithFreeTermQuery : IRequest<IEnumerable<DaysWithFreeTermViewModel>>
{
    public static int DaysAhead => 30;
    public int DoctorId { get; }

    public GetDaysWithFreeTermQuery(int doctorId)
    {
        DoctorId = doctorId;
    }
}

public class GetDaysWithFreeTermQueryHandler
    : IRequestHandler<GetDaysWithFreeTermQuery, IEnumerable<DaysWithFreeTermViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFreeTermService _freeTermService;

    public GetDaysWithFreeTermQueryHandler(IApplicationDbContext context, IFreeTermService freeTermService)
    {
        _context = context;
        _freeTermService = freeTermService;
    }

    public async Task<IEnumerable<DaysWithFreeTermViewModel>> Handle(GetDaysWithFreeTermQuery query,
        CancellationToken cancellationToken)
    {
        if (!_context.Doctors.Any(d => d.Id == query.DoctorId))
        {
            throw new NotFoundException("Doctor not found");
        }

        var maxDayAhead = DateTime.Today.AddDays(GetDaysWithFreeTermQuery.DaysAhead);
        var today = DateTime.Today;
        var visitsToTheDoctor = await _context.Visits
            .Where(v => v.DoctorId == query.DoctorId &&
                        v.DateTime.Date > today.Date &&
                        v.DateTime.Date <= maxDayAhead)
            .OrderBy(v => v.DateTime)
            .ToListAsync(cancellationToken);

        var doctorSchedule = await _context.Schedules
            .Where(s => s.Doctor.Id == query.DoctorId &&
                        s.StartDate.Date > today.Date &&
                        s.StartDate.Date <= maxDayAhead)
            .OrderBy(s => s.StartDate)
            .ToListAsync(cancellationToken);

        var viewModels = _freeTermService
            .GetDaysWithFreeTerm(GetDaysWithFreeTermQuery.DaysAhead, doctorSchedule, visitsToTheDoctor);

        return viewModels;
    }
}