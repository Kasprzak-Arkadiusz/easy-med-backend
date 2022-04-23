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
    public string FirstName { get; }
    public string LastName { get; }
    public string EmailAddress { get; }
    public string Password { get; }
    public Role Role { get; }

    public RegisterUserCommand(string firstName, string lastName, string emailAddress, string password, Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        Role = role;
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

        User user = command.Role switch
        {
            Role.Doctor => Doctor.Create(command.FirstName, command.LastName, command.EmailAddress, command.Password),
            Role.Patient => Patient.Create(command.FirstName, command.LastName, command.EmailAddress, command.Password),
            _ => throw new ArgumentOutOfRangeException()
        };

        await _applicationDbContext.Users.AddAsync(user, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserViewModel>(user);
    }
}