using Api.Dtos.Visit;
using EasyMed.Application.Commands;
using EasyMed.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class VisitController : BaseController
{
    /// <summary>
    /// Reserve a visit
    /// </summary>
    /// <returns>A newly created visit</returns>
    /// <response code="201">Visit successfully created</response>
    /// <response code="400">Validation or logic error</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReserveVisitViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Reserve([FromBody] ReserveVisitDto dto)
    {
        var viewModel = await Mediator.Send(new ReserveVisitCommand(dto.VisitDateTime, dto.DoctorId, dto.PatientId));
        return CreatedAtRoute(null, viewModel);
    }

    /// <summary>
    /// Cancel a visit
    /// </summary>
    /// <response code="200">Visit successfully cancelled</response>
    /// <response code="400">Validation or logic error</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel(int id)
    {
        await Mediator.Send(new CancelVisitCommand(id));
        return Ok();
    }
}