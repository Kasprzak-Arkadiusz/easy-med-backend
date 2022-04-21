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
}