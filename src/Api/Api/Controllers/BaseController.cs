using System.Security.Claims;
using EasyMed.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable ConstantNullCoalescingCondition
#pragma warning disable CS8603
#pragma warning disable CS8601
#pragma warning disable CS8618

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected int RequireUserId()
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == default)
        {
            throw new ForbiddenAccessException("You are not authenticated");
        }

        return int.Parse(idClaim.Value);
    }
}