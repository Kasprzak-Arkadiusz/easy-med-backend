using Api.Dtos.User;
using EasyMed.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController : BaseController
{
    [HttpGet("temp")]
    public Task<string> Temp()
    {
        return Task.FromResult("temp");
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var command = new RegisterUserCommand(dto.EmailAddress, dto.Password, dto.RegisterAs);
        var user = await Mediator.Send(command);
        return CreatedAtAction(null, user);
    }
}