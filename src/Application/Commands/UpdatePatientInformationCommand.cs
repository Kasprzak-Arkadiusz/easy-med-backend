using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class UpdatePatientInformationCommand : IRequest<PatientInformationViewModel>
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string PersonalIdentityNumber { get; }

    public UpdatePatientInformationCommand(int id, string firstName, string lastName,
        string email, string telephone, string personalIdentityNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Telephone = telephone;
        PersonalIdentityNumber = personalIdentityNumber;
    }
}

public class UpdatePatientInformationCommandHandler : IRequestHandler<UpdatePatientInformationCommand,
    PatientInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePatientInformationCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientInformationViewModel> Handle(UpdatePatientInformationCommand command,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);
        if (patient == default)
        {
            throw new BadRequestException("Patient with given id does not exist");
        }

        patient.UpdatePersonalInformation(command.FirstName, command.LastName, command.Telephone, command.PersonalIdentityNumber,
            command.Email);
        
        await _context.SaveChangesAsync(cancellationToken);

        var viewModel = _mapper.Map<PatientInformationViewModel>(patient);
        
        return viewModel;
    }
}