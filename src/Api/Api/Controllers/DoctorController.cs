﻿using EasyMed.Application.Queries.Doctors;
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
    [HttpGet("withSpecialization")]
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
    /// Get medial specializations
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
}