using EasyMed.Application.Queries.Patients;
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
    [HttpGet("{id:int}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorReviews(int id)
    {
        var reviews = await Mediator.Send(new GetReviewsByPatientIdQuery(id));
        return Ok(reviews);
    }
}