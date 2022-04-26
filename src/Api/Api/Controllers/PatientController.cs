using Api.Dtos.Patient;
using EasyMed.Application.Commands;
using EasyMed.Application.Queries.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PatientController : BaseController
{
    /// <summary>
    /// Get reviews created by patient
    /// </summary>
    /// <param name="id">Patient id</param>
    /// <response code="200">Successfully returned reviews</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Patient not found</response>
    [Authorize]
    [HttpGet("{id:int}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorReviews(int id)
    {
        var reviews = await Mediator.Send(new GetReviewsByPatientIdQuery(id));
        return Ok(reviews);
    }
    
    /// <summary>
    /// Get patient information
    /// </summary>
    /// <param name="id">Patient id</param>
    /// <response code="200">Successfully returned patient information</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="403">Cannot get not yours information</response>
    /// <response code="404">Patient not found</response>
    [Authorize]
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPatientInformation(int id)
    {
        var viewModel = await Mediator.Send(new GetPatientInformation(RequireUserId(), id));

        return Ok(viewModel);
    }

    /// <summary>
    /// Update patient information
    /// </summary>
    /// <param name="id">Patient id</param>
    /// <response code="200">Successfully updated patient information</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="403">Cannot update not yours information</response>
    [Authorize]
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdatePatientInformation(int id, [FromBody] UpdatePatientInformationDto dto)
    {
        var viewModel = await Mediator.Send(new UpdatePatientInformationCommand(RequireUserId(), id, dto.FirstName, dto.LastName,
            dto.Email, dto.Telephone, dto.PersonalIdentityNumber));

        return Ok(viewModel);
    }
}