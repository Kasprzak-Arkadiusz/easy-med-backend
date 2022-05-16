using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetPrescriptionPatientsQuery : IRequest<IEnumerable<PrescriptionPatientViewModel>>
{
    public int CurrentUserId { get; }
    public int DoctorId { get; }

    public GetPrescriptionPatientsQuery(int currentUserId, int doctorId)
    {
        CurrentUserId = currentUserId;
        DoctorId = doctorId;
    }
}

public class GetPrescriptionPatientsQueryHandler : IRequestHandler<GetPrescriptionPatientsQuery,
    IEnumerable<PrescriptionPatientViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPrescriptionPatientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrescriptionPatientViewModel>> Handle(GetPrescriptionPatientsQuery query,
        CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == query.CurrentUserId, cancellationToken);

        if (doctor == default)
        {
            throw new ForbiddenAccessException("You need to be a doctor to do that");
        }

        AuthorizationService.VerifyIfSameUser(query.CurrentUserId, query.DoctorId, "You cannot get not yours patients");

        var patients = await _context.Patients
            .Where(p => p.Visits.Any(v => v.DoctorId == query.CurrentUserId && v.IsCompleted))
            .Select(p => _mapper.Map<PrescriptionPatientViewModel>(p))
            .ToListAsync(cancellationToken);

        return patients;
    }
}