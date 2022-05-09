using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetPrescriptionsByDoctorIdQuery : IRequest<IEnumerable<DoctorPrescriptionViewModel>>
{
    public int DoctorId { get; }

    public GetPrescriptionsByDoctorIdQuery(int doctorId)
    {
        DoctorId = doctorId;
    }
}

public class GetPrescriptionsByDoctorIdQueryHandler : IRequestHandler<GetPrescriptionsByDoctorIdQuery,
    IEnumerable<DoctorPrescriptionViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPrescriptionsByDoctorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorPrescriptionViewModel>> Handle(GetPrescriptionsByDoctorIdQuery query,
        CancellationToken cancellationToken)
    {
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == query.DoctorId, cancellationToken);

        if (!doctorExists)
        {
            throw new NotFoundException("Doctor not found");
        }

        var prescriptions = await _context.Prescriptions
            .Where(p => p.DoctorId == query.DoctorId)
            .Include(p => p.Patient)
            .Include(p => p.Medicines)
            .OrderByDescending(p => p.DateOfIssue)
            .Select(p => _mapper.Map<DoctorPrescriptionViewModel>(p))
            .ToListAsync(cancellationToken);

        return prescriptions;
    }
}