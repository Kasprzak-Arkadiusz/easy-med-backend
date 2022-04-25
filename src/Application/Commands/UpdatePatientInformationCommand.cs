using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class UpdatePatientInformationCommand : IRequest<UpdatedPatientInformationViewModel>
{
    public int CurrentUserId { get; }
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Pesel { get; }

    public UpdatePatientInformationCommand(int currentUserId, int id, string firstName, string lastName,
        string email, string telephone, string pesel)
    {
        CurrentUserId = currentUserId;
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Telephone = telephone;
        Pesel = pesel;
    }
}

public class UpdatePatientInformationCommandHandler : IRequestHandler<UpdatePatientInformationCommand,
    UpdatedPatientInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePatientInformationCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdatedPatientInformationViewModel> Handle(UpdatePatientInformationCommand command,
        CancellationToken cancellationToken)
    {
        Authorize(command.Id, command.CurrentUserId);
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);
        if (patient == default)
        {
            throw new BadRequestException("Patient with given id does not exist");
        }

        patient.UpdatePersonalInformation(command.FirstName, command.LastName, command.Telephone, command.Pesel,
            command.Email);
        
        await _context.SaveChangesAsync(cancellationToken);

        var viewModel = _mapper.Map<UpdatedPatientInformationViewModel>(patient);
        
        return viewModel;
    }

    private static void Authorize(int id, int currentUserId)
    {
        if (id != currentUserId)
        {
            throw new ForbiddenAccessException("You are not authorized");
        }
    }
}