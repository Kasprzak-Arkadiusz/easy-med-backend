using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Services;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace EasyMed.Application.Queries.Doctors;

public class GetDailyInfoByDoctorIdQuery : IRequest<DailyInfoViewModel>
{
    public int CurrentUserId { get; }
    public int DoctorId { get; }

    public GetDailyInfoByDoctorIdQuery(int currentUserId, int doctorId)
    {
        CurrentUserId = currentUserId;
        DoctorId = doctorId;
    }
}

public class GetDailyInfoByDoctorIdQueryHandler : IRequestHandler<GetDailyInfoByDoctorIdQuery, DailyInfoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetDailyInfoByDoctorIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DailyInfoViewModel> Handle(GetDailyInfoByDoctorIdQuery query, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == query.DoctorId, cancellationToken);
        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }

        AuthorizationService.VerifyIfSameUser(query.DoctorId, query.CurrentUserId,
            "You cannot see a schedule that is not yours");

        int remainingVisitsCount = await CountRemainingVisitsTodayAsync(query.DoctorId, cancellationToken);
        int? averageRating = await CalculateAverageRatingAsync(query.DoctorId, cancellationToken);
        int issuedPrescriptionsCount = await CountIssuedPrescriptionsTodayAsync(query.DoctorId, cancellationToken);
        var endOfWorkTime = await GetEndOfWorkTimeAsync(query.DoctorId, cancellationToken);


        return new DailyInfoViewModel
        {
            CurrentRating = averageRating,
            RemainingVisits = remainingVisitsCount,
            IssuedPrescriptions = issuedPrescriptionsCount,
            EndOfWorkAt = endOfWorkTime
        };
    }

    private async Task<DateTime?> GetEndOfWorkTimeAsync(int doctorId,
        CancellationToken cancellationToken)
    {
        var endDates = await _context.Visits
            .Where(v => v.DoctorId == doctorId && v.DateTime.Date == DateTime.Today.Date)
            .OrderBy(v => v.DateTime)
            .Select(v => v.DateTime)
            .ToListAsync(cancellationToken);

        return endDates.Any() ? endDates.Last().AddMinutes(Visit.GetVisitTimeInMinutes()) : null;
    }

    private async Task<int> CountIssuedPrescriptionsTodayAsync(int doctorId, CancellationToken cancellationToken)
    {
        var date = DateOnly.FromDateTime(DateTime.Today);
        return await _context.Prescriptions
            .Where(p => p.DoctorId == doctorId && p.DateOfIssue == date)
            .CountAsync(cancellationToken);
    }

    private async Task<int> CountRemainingVisitsTodayAsync(int doctorId, CancellationToken cancellationToken)
    {
        var remainingVisits = await _context.Visits
            .Where(v => v.DoctorId == doctorId
                        && v.DateTime.Date == DateTime.Today
                        && v.DateTime > DateTime.Now)
            .CountAsync(cancellationToken);
        return remainingVisits;
    }

    private async Task<int?> CalculateAverageRatingAsync(int doctorId, CancellationToken cancellationToken)
    {
        var ratings = await _context.Reviews
            .Where(r => r.DoctorId == doctorId)
            .Select(r => (int)r.Rating)
            .ToListAsync(cancellationToken);

        int? averageRating = ratings.Any()
            ? (int)Math.Round(ratings.Average())
            : null;
        return averageRating;
    }
}