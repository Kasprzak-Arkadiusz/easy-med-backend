using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class ReserveVisitCommand : IRequest<ReserveVisitViewModel>
{
    public DateTime VisitDateTime { get; }
    public int DoctorId { get; }
    public int PatientId { get; }

    public ReserveVisitCommand(DateTime visitDateTime, int doctorId, int patientId)
    {
        VisitDateTime = visitDateTime;
        DoctorId = doctorId;
        PatientId = patientId;
    }
}

public class ReserveVisitCommandHandler : IRequestHandler<ReserveVisitCommand, ReserveVisitViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ReserveVisitCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReserveVisitViewModel> Handle(ReserveVisitCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .Include(d => d.OfficeLocation)
            .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new BadRequestException("Doctor with given id does not exist");
        }

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);
        if (patient == default)
        {
            throw new BadRequestException("Patient with given id does not exist");
        }

        if (request.VisitDateTime <= DateTime.Now)
        {
            throw new BadRequestException("You can make an appointment the day before the visit at the latest");
        }

        var sameVisitExists = _context.Visits.Any(v =>
            v.DateTime == request.VisitDateTime &&
            v.DoctorId == request.DoctorId);

        if (sameVisitExists)
        {
            throw new BadRequestException("Visit cannot be reserved. This term is busy");
        }

        var visit = Visit.Create(request.VisitDateTime, doctor, patient);

        await _context.Visits.AddAsync(visit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var viewModel = _mapper.Map<ReserveVisitViewModel>(visit);

        return viewModel;
    }
}