using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using EasyMed.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class CreateReviewCommand : IRequest<ReviewViewModel>
{
    public int CurrentUserId { get; }
    public int DoctorId { get; }
    public string Description { get; }
    public short Rating { get; }

    public CreateReviewCommand(int currentUserId, int doctorId, string description, short rating)
    {
        CurrentUserId = currentUserId;
        DoctorId = doctorId;
        Description = description;
        Rating = rating;
    }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReviewViewModel> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        await VerifyIfCurrentUserExistsAndHasPatientRole(command, cancellationToken);
        Patient patient = await GetPatientOrThrowNotFoundException(command, cancellationToken);
        Doctor doctor = await GetDoctorOrThrowNotFoundException(command, cancellationToken);

        var review = Review.Create(command.Description, command.Rating, doctor, patient);
        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReviewViewModel>(review);
    }

    private async Task VerifyIfCurrentUserExistsAndHasPatientRole(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == command.CurrentUserId, cancellationToken);

        if (currentUser == default)
        {
            throw new NotFoundException("User not found");
        }

        if (currentUser.Role != Role.Patient)
        {
            throw new ForbiddenAccessException("You need to be patient to create a review");
        }
    }

    private async Task<Doctor> GetDoctorOrThrowNotFoundException(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(p => p.Id == command.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        return doctor;
    }

    private async Task<Patient> GetPatientOrThrowNotFoundException(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == command.CurrentUserId, cancellationToken);
        if (patient == default)
        {
            throw new NotFoundException("Patient not found");
        }

        return patient;
    }
}