using EasyMed.Application.Common.Exceptions;
using EasyMed.Application.Common.Interfaces;
using EasyMed.Application.Utils.SecurityTokens;
using EasyMed.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace EasyMed.Application.Commands;

public class LoginUserCommand : IRequest<AuthViewModel>
{
    public string EmailAddress { get; }
    public string Password { get; }

    public LoginUserCommand(string emailAddress, string password)
    {
        EmailAddress = emailAddress;
        Password = password;
    }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthViewModel>
{
    private readonly ISecurityTokenService _securityTokenService;
    private readonly IApplicationDbContext _applicationDbContext;

    public LoginUserCommandHandler(ISecurityTokenService securityTokenService,
        IApplicationDbContext applicationDbContext)
    {
        _securityTokenService = securityTokenService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<AuthViewModel> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.EmailAddress.Equals(command.EmailAddress), cancellationToken);
        
        if (user == default)
        {
            throw new NotFoundException("User not found");
        }

        if (!Verify(command.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        var accessToken = _securityTokenService.GenerateAccessTokenForUser(user.Id, user.EmailAddress, user.Role);

        return new AuthViewModel
        {
            AccessToken = accessToken,
            Role = user.Role.ToString(),
            EmailAddress = user.EmailAddress
        };
    }
}