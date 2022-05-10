using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class CreatePrescriptionCommand : IRequest<DoctorPrescriptionViewModel>
{
    public int CurrentUserId { get; }
    public int Id { get; }
    public int PatientId { get; }
    public DateOnly DateOfIssue { get; }
    public IEnumerable<CreateMedicineViewModel> Medicines { get; }

    public CreatePrescriptionCommand(int currentUserId, int id, int patientId, DateOnly dateOfIssue,
        IEnumerable<CreateMedicineViewModel> medicines)
    {
        CurrentUserId = currentUserId;
        Id = id;
        PatientId = patientId;
        DateOfIssue = dateOfIssue;
        Medicines = medicines;
    }
}

public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, DoctorPrescriptionViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePrescriptionCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DoctorPrescriptionViewModel> Handle(CreatePrescriptionCommand command,
        CancellationToken cancellationToken)
    {
        var currentUser =
            await _context.Doctors.FirstOrDefaultAsync(d => d.Id == command.CurrentUserId, cancellationToken);

        if (currentUser == default)
        {
            throw new ForbiddenAccessException("You need to be a doctor to do that");
        }

        AuthorizationService.VerifyIfSameUser(command.Id, command.CurrentUserId,
            "You cannot create prescription not for your patient");

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == command.PatientId, cancellationToken);

        if (patient == default)
        {
            throw new NotFoundException("Patient not found");
        }

        var medicines = command.Medicines.Select(medicine => Medicine.Create(medicine.Name, medicine.Capacity))
            .ToList();

        var prescription = Prescription.Create(command.DateOfIssue, currentUser, patient, medicines);

        await _context.Prescriptions.AddAsync(prescription, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var viewModel = _mapper.Map<DoctorPrescriptionViewModel>(prescription);

        return viewModel;
    }
}