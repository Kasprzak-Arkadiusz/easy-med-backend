using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Enums;
using EasyMed.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class UpdateDoctorInformationCommand : IRequest<DoctorInformationViewModel>
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Telephone { get; }
    public string Description { get; }
    public string OfficeLocation { get; }
    public MedicalSpecialization? MedicalSpecialization { get; }

    public UpdateDoctorInformationCommand(int id, string firstName, string lastName, string email,
        string telephone, string description, string officeLocation, MedicalSpecialization? medicalSpecialization)
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

public class
    UpdateDoctorInformationCommandHandler : IRequestHandler<UpdateDoctorInformationCommand, DoctorInformationViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateDoctorInformationCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DoctorInformationViewModel> Handle(UpdateDoctorInformationCommand command,
        CancellationToken cancellationToken)
    {
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

        var viewModel = _mapper.Map<DoctorInformationViewModel>(doctor);

        return viewModel;
    }
}