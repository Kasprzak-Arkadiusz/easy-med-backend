using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.GetDoctors;

public class GetFreeTermsByDoctorIdQuery : IRequest<IEnumerable<FreeTermViewModel>>
{
    public int DoctorId { get; }
    public DateTime VisitDate { get; }

    public GetFreeTermsByDoctorIdQuery(int doctorId, DateTime visitDateTime)
    {
        DoctorId = doctorId;
        VisitDate = visitDateTime;
    }
}

public class GetFreeTermsByDoctorIdQueryHandler : IRequestHandler<GetFreeTermsByDoctorIdQuery,
    IEnumerable<FreeTermViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFreeTermService _freeTermService;

    public GetFreeTermsByDoctorIdQueryHandler(IApplicationDbContext context, IFreeTermService freeTermService)
    {
        _context = context;
        _freeTermService = freeTermService;
    }

    public async Task<IEnumerable<FreeTermViewModel>> Handle(GetFreeTermsByDoctorIdQuery request,
        CancellationToken cancellationToken)
    {
        if (!_context.Doctors.Any(d => d.Id == request.DoctorId))
            throw new NotFoundException("Doctor not found");

        if (request.VisitDate <= DateTime.Now)
            throw new BadRequestException("You can make an appointment the day before the visit at the latest");

        var visitsToTheDoctor = await _context.Visits
            .Where(v => v.DoctorId == request.DoctorId && v.DateTime.Date == request.VisitDate.Date)
            .ToListAsync(cancellationToken);

        var doctorSchedule = _context.Schedules
            .FirstOrDefault(s => s.DayOfWeek == request.VisitDate.DayOfWeek.ToString() 
                                 && s.Doctor.Id == request.DoctorId);

        var freeTerms = _freeTermService.CalculateFreeTerms(request.VisitDate, doctorSchedule, visitsToTheDoctor);

        return freeTerms;
    }
}