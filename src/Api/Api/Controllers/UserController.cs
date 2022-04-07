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
    /// <summary>
    /// Creates either Doctor or Patient
    /// </summary>
    /// <returns>A newly created User</returns>
    /// <response code="201">User successfully registered</response>
    /// <response code="400">Validation or logic error</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var command = new RegisterUserCommand(dto.EmailAddress, dto.Password, dto.RegisterAs);
        var user = await Mediator.Send(command);
        return CreatedAtAction(null, user);
    }
}