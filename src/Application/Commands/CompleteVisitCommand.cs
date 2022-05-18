using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class CompleteVisitCommand : IRequest<Unit>
{
    public int CurrentUserId { get; }
    public int VisitId { get; }

    public CompleteVisitCommand(int currentUserId, int visitId)
    {
        CurrentUserId = currentUserId;
        VisitId = visitId;
    }
}

public class CompleteVisitCommandHandler : IRequestHandler<CompleteVisitCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public CompleteVisitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CompleteVisitCommand command, CancellationToken cancellationToken)
    {
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == command.CurrentUserId, cancellationToken);

        if (!doctorExists)
        {
            throw new ForbiddenAccessException("You need to be a doctor to do that");
        }

        var visit = await _context.Visits.FirstOrDefaultAsync(v => v.Id == command.VisitId, cancellationToken);

        if (visit == default)
        {
            throw new NotFoundException("Visit with provided id does not exist");
        }

        var currentUserIsRequestedVisitDoctor = visit.DoctorId == command.CurrentUserId;

        if (!currentUserIsRequestedVisitDoctor)
        {
            throw new ForbiddenAccessException("You cannot complete not your visit");
        }

        visit.Complete();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}