using EasyMed.Application.Queries.GetDoctors;
using EasyMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : BaseController
{
    /// <summary>
    /// Get doctors with a requested specialization
    /// </summary>
    /// <param name="specialization">Medical specialization name</param> 
    /// <returns>Doctors with requested specialization</returns>
    /// <response code="200">Successfully returned doctors</response>
    /// <response code="400">Validation or logic error (e.g. MedicalSpecialization does not exist)</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByMedicalSpecialization(MedicalSpecialization specialization)
    {
        try
        {
            var doctors = await Mediator.Send(new GetDoctorsByMedicalSpecializationQuery(specialization));
            return Ok(doctors);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}