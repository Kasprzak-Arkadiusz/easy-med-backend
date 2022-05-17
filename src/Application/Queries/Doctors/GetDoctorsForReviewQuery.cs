using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetDoctorsForReviewQuery : IRequest<IEnumerable<DoctorForReviewViewModel>>
{
    public int CurrentUserId { get; }
    public int PatientId { get; }

    public GetDoctorsForReviewQuery(int currentUserId, int patientId)
    {
        CurrentUserId = currentUserId;
        PatientId = patientId;
    }
}

public class
    GetDoctorsForReviewQueryHandler : IRequestHandler<GetDoctorsForReviewQuery, IEnumerable<DoctorForReviewViewModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDoctorsForReviewQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorForReviewViewModel>> Handle(GetDoctorsForReviewQuery query,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == query.CurrentUserId, cancellationToken);

        if (patient == default)
        {
            throw new ForbiddenAccessException("You need to be a patient to do that");
        }

        AuthorizationService.VerifyIfSameUser(query.CurrentUserId, query.PatientId,
            "You cannot get information about not your reviews");

        var doctors = await _context.Doctors
            .Include(d => d.Visits)
            .Where(d => d.Visits.Any(v => v.PatientId == query.PatientId && v.IsCompleted) &&
                        d.Reviews.All(r => r.PatientId != query.PatientId))
            .Select(d => new DoctorForReviewViewModel
            {
                Doctor = new ReviewDoctorViewModel
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    MedicalSpecialization = d.MedicalSpecialization
                },
                DateOfVisit = d.Visits.OrderByDescending(v => v.DateTime).First().DateTime
            }).ToListAsync(cancellationToken);

        return doctors;
    }
}