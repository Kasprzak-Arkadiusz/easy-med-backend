using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetVisitsByPatientIdQuery : IRequest<IEnumerable<PatientVisitViewModel>>
{
    public int PatientId { get; }
    public bool? IsCompleted { get; }

    public GetVisitsByPatientIdQuery(int patientId, bool? isCompleted)
    {
        PatientId = patientId;
        IsCompleted = isCompleted;
    }
}

public class
    GetVisitsByPatientIdQueryHandler : IRequestHandler<GetVisitsByPatientIdQuery, IEnumerable<PatientVisitViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVisitsByPatientIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientVisitViewModel>> Handle(GetVisitsByPatientIdQuery query,
        CancellationToken cancellationToken)
    {
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Id == query.PatientId, cancellationToken);
        if (!patientExists)
        {
            throw new NotFoundException("Patient not found");
        }

        var visits = await _context.Visits
            .Where(v => v.PatientId == query.PatientId)
            .Where(v => query.IsCompleted == null || v.IsCompleted == query.IsCompleted)
            .Include(v => v.Doctor)
            .ThenInclude(d => d.OfficeLocation)
            .OrderBy(v => v.DateTime)
            .Select(v => _mapper.Map<PatientVisitViewModel>(v))
            .ToListAsync(cancellationToken);

        return visits;
    }
}