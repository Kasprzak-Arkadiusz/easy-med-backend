using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetDoctorInformationQuery : IRequest<DoctorInformationViewModel>
{
    public int Id { get; }

    public GetDoctorInformationQuery(int id)
    {
        Id = id;
    }
}

public class GetDoctorInformationQueryHandler : IRequestHandler<GetDoctorInformationQuery, DoctorInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDoctorInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DoctorInformationViewModel> Handle(GetDoctorInformationQuery query,
        CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.OfficeLocation)
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        var viewModel = _mapper.Map<DoctorInformationViewModel>(doctor);

        return viewModel;
    }
}