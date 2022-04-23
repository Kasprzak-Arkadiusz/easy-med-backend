using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Queries.Doctors;

public class GetReviewsByDoctorIdQuery : IRequest<IEnumerable<ReviewViewModel>>
{
    public int DoctorId { get; }

    public GetReviewsByDoctorIdQuery(int doctorId)
    {
        DoctorId = doctorId;
    }
}

public class GetReviewsByDoctorIdQueryValidator 
    : IRequestHandler<GetReviewsByDoctorIdQuery, IEnumerable<ReviewViewModel>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetReviewsByDoctorIdQueryValidator(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReviewViewModel>> Handle(GetReviewsByDoctorIdQuery query,
        CancellationToken cancellationToken)
    {
        var doctor = await _dbContext.Doctors
            .FirstOrDefaultAsync(d => d.Id == query.DoctorId, cancellationToken);

        if (doctor == default)
        {
            throw new NotFoundException("Doctor not found");
        }
        
        var reviews = await _dbContext.Reviews
            .Where(r => r.DoctorId == query.DoctorId)
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Select(r => _mapper.Map<ReviewViewModel>(r))
            .ToListAsync(cancellationToken);

        return reviews;
    }
}