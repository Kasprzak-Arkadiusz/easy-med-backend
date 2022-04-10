using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using EasyMed.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CA2208

namespace EasyMed.Application.Commands;

public class RegisterUserCommand : IRequest<UserViewModel>
{
    public string EmailAddress { get; }
    public string Password { get; }
    public Role RegisterAs { get; }

    public RegisterUserCommand(string emailAddress, string password, Role registerAs)
    {
        EmailAddress = emailAddress;
        Password = password;
        RegisterAs = registerAs;
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.EmailAddress == command.EmailAddress, cancellationToken);

        if (existingUser != default)
        {
            throw new BadRequestException("User with given email address already exists");
        }

        User user = command.RegisterAs switch
        {
            Role.Doctor => Doctor.Create(command.EmailAddress, command.Password),
            Role.Patient => Patient.Create(command.EmailAddress, command.Password),
            _ => throw new ArgumentOutOfRangeException()
        };

        await _applicationDbContext.Users.AddAsync(user, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<UserViewModel>(user);
    }
}