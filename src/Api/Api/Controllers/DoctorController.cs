using Api.Dtos.Doctor;
using EasyMed.Application.Commands;
using EasyMed.Application.Queries.Doctors;
using EasyMed.Application.ViewModels;
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
    public async Task<ActionResult<IEnumerable<DoctorViewModel>>> GetByMedicalSpecialization(
        MedicalSpecialization specialization)
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

    /// <summary>
    /// Get free terms for a specific doctor
    /// </summary>
    /// <param name="doctorId">Doctor id</param> 
    /// <param name="visitDateTime">DateTime of visit (e.g. 2017-07-21T17:32:28Z)</param> 
    /// <returns>Free terms for a specific doctor</returns>
    /// <response code="200">Successfully returned free terms</response>
    /// <response code="400">Validation or logic error</response>
    [HttpGet("freeTerms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FreeTermViewModel>>> GetDoctorFreeTerm(int doctorId,
        DateTime visitDateTime)
    {
        try
        {
            var doctors = await Mediator.Send(new GetFreeTermsByDoctorIdQuery(doctorId, visitDateTime));
            return Ok(doctors);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get medical specializations
    /// </summary>
    /// <returns>Medical specializations</returns>
    /// <response code="200">Successfully returned medical specializations</response>
    [HttpGet("specializations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalSpecialization>>> GetMedicalSpecializations()
    {
        var medicalSpecializations = await Mediator.Send(new GetMedicalSpecializationsQuery());
        return Ok(medicalSpecializations);
    }

    /// <summary>
    /// Update doctor information
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <response code="200">Successfully returned medical specializations</response>
    /// <response code="400">Validation or logic error</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateDoctorInformation(int id, [FromBody] UpdateDoctorInformationDto dto)
    {
        await Mediator.Send(new UpdateDoctorInformationCommand(id, dto.FirstName, dto.LastName, dto.Email,
            dto.Telephone, dto.Description, dto.OfficeLocation, dto.MedicalSpecialization));

        return Ok();
    }
    
    /// <summary>
    /// Get doctor reviews
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <response code="200">Successfully returned reviews</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [HttpGet("{id:int}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorReviews(int id)
    {
        var reviews = await Mediator.Send(new GetReviewsByDoctorIdQuery(id));
        return Ok(reviews);
    }
}