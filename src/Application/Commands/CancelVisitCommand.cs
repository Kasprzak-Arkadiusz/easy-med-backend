using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyMed.Application.Commands;

public class CancelVisitCommand : IRequest<Unit>
{
    public int Id { get; }

    public CancelVisitCommand(int id)
    {
        Id = id;
    }
}

public class CancelVisitCommandHandler : IRequestHandler<CancelVisitCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public CancelVisitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CancelVisitCommand request, CancellationToken cancellationToken)
    {
        var visit = await _context.Visits.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        if (visit == default)
        {
            throw new BadRequestException("Visit with given id does not exist");
        }
        
        _context.Visits.Remove(visit);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}