﻿using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetPrescriptionsByPatientIdQuery : IRequest<IEnumerable<PrescriptionViewModel>>
{
    public int CurrentUserId { get; }
    public int PatientId { get; }

    public GetPrescriptionsByPatientIdQuery(int currentUserId, int patientId)
    {
        CurrentUserId = currentUserId;
        PatientId = patientId;
    }
}

public class GetPrescriptionsByPatientIdQueryHandler
    : IRequestHandler<GetPrescriptionsByPatientIdQuery, IEnumerable<PrescriptionViewModel>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetPrescriptionsByPatientIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrescriptionViewModel>> Handle(GetPrescriptionsByPatientIdQuery query,
        CancellationToken cancellationToken)
    {
        AuthorizationService.VerifyIfSameUser(query.PatientId, query.CurrentUserId, "You cannot get not yours prescriptions");

        var patient = await _dbContext.Patients
            .FirstOrDefaultAsync(d => d.Id == query.PatientId, cancellationToken);

        if (patient == default)
        {
            throw new NotFoundException("Patient not found");
        }

        var prescriptions = await _dbContext.Prescriptions
            .Where(p => p.PatientId == query.PatientId)
            .OrderByDescending(p => p.DateOfIssue)
            .Include(p => p.Doctor)
            .ThenInclude(d => d.OfficeLocation)
            .Include(p => p.Medicines)
            .Select(p => _mapper.Map<PrescriptionViewModel>(p))
            .ToListAsync(cancellationToken);

        return prescriptions;
    }
}