﻿using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetVisitsByDoctorIdQuery : IRequest<IEnumerable<VisitViewModel>>
{
    public int DoctorId { get; }
    public bool? IsCompleted { get; }

    public GetVisitsByDoctorIdQuery(int doctorId, bool? isCompleted)
    {
        DoctorId = doctorId;
        IsCompleted = isCompleted;
    }
}

public class GetVisitsByDoctorIdQueryHandler : IRequestHandler<GetVisitsByDoctorIdQuery, IEnumerable<VisitViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVisitsByDoctorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VisitViewModel>> Handle(GetVisitsByDoctorIdQuery query,
        CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == query.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        var visits = await _context.Visits
            .Where(v => v.Doctor.Id == query.DoctorId)
            .Where(v => query.IsCompleted == null || v.IsCompleted == query.IsCompleted)
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .ThenInclude(d => d.OfficeLocation)
            .OrderByDescending(v => v.DateTime)
            .Select(v => _mapper.Map<VisitViewModel>(v))
            .ToListAsync(cancellationToken);

        return visits;
    }
}