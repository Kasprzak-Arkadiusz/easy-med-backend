using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Domain.Enums;
using EasyMed.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class UpdateDoctorInformationCommand : IRequest<bool>
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Description { get; }
    public string OfficeLocation { get; }
    public MedicalSpecialization? MedicalSpecialization { get; }

    public UpdateDoctorInformationCommand(int id, string firstName, string lastName, string email, string telephone,
        string description, string officeLocation, MedicalSpecialization? medicalSpecialization)
    {
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

public class UpdateDoctorInformationCommandHandler : IRequestHandler<UpdateDoctorInformationCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateDoctorInformationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateDoctorInformationCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.OfficeLocation)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        if (doctor == default)
        {
            return false;
        }

        doctor.UpdatePersonalInformation(request.FirstName, request.LastName, request.Telephone, request.Description,
            request.Email);
        doctor.ChangeMedicalSpecialization(request.MedicalSpecialization);
        
        if (!string.IsNullOrEmpty(request.OfficeLocation))
        {
            try
            {
                doctor.UpdateOfficeLocation(request.OfficeLocation);
            }
            catch (MissingAddressDetailsException e)
            {
                throw new BadRequestException(e.Message);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}