using AutoMapper;
using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Utils.SecurityTokens;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Entities;
using EasyMed.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA2208

namespace EasyMed.Application.Commands;

public class RegisterUserCommand : IRequest<AuthViewModel>
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

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthViewModel>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ISecurityTokenService _securityTokenService;
    
    public RegisterUserCommandHandler(IApplicationDbContext applicationDbContext, ISecurityTokenService securityTokenService)
    {
        _applicationDbContext = applicationDbContext;
        _securityTokenService = securityTokenService;
    }

    public async Task<AuthViewModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
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

        var accessToken = _securityTokenService.GenerateAccessTokenForUser(user.Id, user.EmailAddress, user.Role);

        return new AuthViewModel
        {
            AccessToken = accessToken,
            Id = user.Id,
            Role = user.Role.ToString(),
            EmailAddress = user.EmailAddress,
            FirstName = user.FirstName!,
            LastName = user.LastName!
        };
    }
}