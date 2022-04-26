using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetPatientInformation : IRequest<PatientInformationViewModel>
{
    public int CurrentUserId { get; }
    public int Id { get; }

    public GetPatientInformation(int currentUserId, int id)
    {
        CurrentUserId = currentUserId;
        Id = id;
    }
}

public class GetPatientInformationHandler : IRequestHandler<GetPatientInformation, PatientInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientInformationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientInformationViewModel> Handle(GetPatientInformation query,
        CancellationToken cancellationToken)
    {
        AuthorizationService.VerifyIfSameUser(query.Id, query.CurrentUserId, "You cannot get not yours information");
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