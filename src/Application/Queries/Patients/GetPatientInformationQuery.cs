using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetPatientInformationQuery : IRequest<PatientInformationViewModel>
{
    public int Id { get; }

    public GetPatientInformationQuery(int id)
    {
        Id = id;
    }
}

public class
    GetPatientInformationQueryHandler : IRequestHandler<GetPatientInformationQuery, PatientInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientInformationViewModel> Handle(GetPatientInformationQuery query,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(d => d.Id == query.Id, cancellationToken);
        if (patient == default)
        {
            throw new NotFoundException("Patient not found");
        }

        var viewModel = _mapper.Map<PatientInformationViewModel>(patient);

        return viewModel;
    }
}