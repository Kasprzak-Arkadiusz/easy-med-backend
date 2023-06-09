﻿using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Patients;

public class GetReviewsByPatientIdQuery : IRequest<IEnumerable<ReviewViewModel>>
{
    public int CurrentUserId { get; }
    public int PatientId { get; }

    public GetReviewsByPatientIdQuery(int currentUserId, int patientId)
    {
        CurrentUserId = currentUserId;
        PatientId = patientId;
    }
}

public class GetReviewsByPatientIdQueryValidator 
    : IRequestHandler<GetReviewsByPatientIdQuery, IEnumerable<ReviewViewModel>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetReviewsByPatientIdQueryValidator(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReviewViewModel>> Handle(GetReviewsByPatientIdQuery query,
        CancellationToken cancellationToken)
    {
        AuthorizationService.VerifyIfSameUser(query.PatientId, query.CurrentUserId, "You cannot get not yours prescriptions");

        var patient = await _dbContext.Patients
            .FirstOrDefaultAsync(d => d.Id == query.PatientId, cancellationToken);

        if (patient == default)
        {
            throw new NotFoundException("Patient not found");
        }
        
        var reviews = await _dbContext.Reviews
            .Where(r => r.PatientId == query.PatientId)
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .ThenInclude(d => d.OfficeLocation)
            .Select(r => _mapper.Map<ReviewViewModel>(r))
            .ToListAsync(cancellationToken);

        return reviews;
    }
}