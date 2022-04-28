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
    /// <response code="200">Successfully returned patient information</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Patient not found</response>
    [Authorize]
    [HttpGet("details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPatientInformation()
    {
        var viewModel = await Mediator.Send(new GetPatientInformationQuery(RequireUserId()));

        return Ok(viewModel);
    }

    /// <summary>
    /// Update patient information
    /// </summary>
    /// <response code="200">Successfully updated patient information</response>
    /// <response code="400">Validation or logic error</response>
    [Authorize]
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePatientInformation([FromBody] UpdatePatientInformationDto dto)
    {
        var viewModel = await Mediator.Send(new UpdatePatientInformationCommand(RequireUserId(), dto.FirstName, dto.LastName,
            dto.Email, dto.Telephone, dto.PersonalIdentityNumber));

        return Ok(viewModel);
    }
}