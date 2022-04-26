using Api.Dtos.User;
using EasyMed.Application.Commands;
using EasyMed.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController : BaseController
{
    /// <summary>
    /// Register either Doctor or Patient
    /// </summary>
    /// <returns>A newly created User</returns>
    /// <response code="201">User successfully registered</response>
    /// <response code="400">Validation or logic error</response>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthViewModel), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var user = await Mediator.Send(new RegisterUserCommand(dto.FirstName, dto.LastName, dto.EmailAddress,
            dto.Password, dto.Role));
        return CreatedAtRoute(null, user);
    }

    /// <summary>
    /// Login either Doctor or Patient
    /// </summary>
    /// <returns>An access token, user role and user email address</returns>
    /// <response code="200">User successfully logged in</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="401">Invalid credentials</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthViewModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType( 401)]
    public async Task<ActionResult<AuthViewModel>> Login([FromBody] LoginUserDto dto)
    {
        var auth = await Mediator.Send(new LoginUserCommand(dto.EmailAddress, dto.Password, dto.Role));
        return Ok(auth);
    }
}