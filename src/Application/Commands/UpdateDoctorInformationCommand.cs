using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Enums;
using EasyMed.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class UpdateDoctorInformationCommand : IRequest<Unit>
{
    public int CurrentUserId { get; }
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Description { get; }
    public string OfficeLocation { get; }
    public MedicalSpecialization? MedicalSpecialization { get; }

    public UpdateDoctorInformationCommand(int currentUserId, int id, string firstName, string lastName, string email,
        string telephone, string description, string officeLocation, MedicalSpecialization? medicalSpecialization)
    {
        CurrentUserId = currentUserId;
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Telephone = telephone;
        Description = description;
        OfficeLocation = officeLocation;
        MedicalSpecialization = medicalSpecialization;
    }
}

public class UpdateDoctorInformationCommandHandler : IRequestHandler<UpdateDoctorInformationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateDoctorInformationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDoctorInformationCommand command, CancellationToken cancellationToken)
    {
        Authorize(command.Id, command.CurrentUserId);
        var doctor = await _context.Doctors
            .Include(d => d.OfficeLocation)
            .FirstOrDefaultAsync(d => d.Id == command.Id, cancellationToken);
        if (doctor == default)
        {
            throw new BadRequestException("Doctor with given id does not exist");
        }

        doctor.UpdatePersonalInformation(command.FirstName, command.LastName, command.Telephone, command.Description,
            command.Email);
        doctor.ChangeMedicalSpecialization(command.MedicalSpecialization);

        if (!string.IsNullOrEmpty(command.OfficeLocation))
        {
            try
            {
                doctor.UpdateOfficeLocation(command.OfficeLocation);
            }
            catch (MissingAddressDetailsException e)
            {
                throw new BadRequestException(e.Message);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private static void Authorize(int id, int currentUserId)
    {
        if (id != currentUserId)
        {
            throw new ForbiddenAccessException("You are not authorized");
        }
    }
}