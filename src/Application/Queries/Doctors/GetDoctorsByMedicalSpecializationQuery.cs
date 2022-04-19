using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetDoctorsByMedicalSpecializationQuery : IRequest<IEnumerable<DoctorViewModel>>
{
    public MedicalSpecialization MedicalSpecialization { get; }

    public GetDoctorsByMedicalSpecializationQuery(MedicalSpecialization medicalSpecialization)
    {
        MedicalSpecialization = medicalSpecialization;
    }
}

public class GetDoctorByMedicalSpecializationQueryHandler : IRequestHandler<GetDoctorsByMedicalSpecializationQuery,
    IEnumerable<DoctorViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDoctorByMedicalSpecializationQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _context = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorViewModel>> Handle(GetDoctorsByMedicalSpecializationQuery request,
        CancellationToken cancellationToken)
    {
        var doctorsVm = await _context.Doctors
            .Include(d => d.OfficeLocation)
            .Where(d => d.MedicalSpecialization == request.MedicalSpecialization.ToString())
            .ProjectTo<DoctorViewModel>(_mapper.ConfigurationProvider)
            .OrderBy(vm => vm.LastName)
            .ToListAsync(cancellationToken);

        return doctorsVm;
    }
}