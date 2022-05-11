using Api.Dtos.Doctor;
using EasyMed.Application.Commands;
using EasyMed.Application.Queries.Doctors;
using EasyMed.Application.ViewModels;
using EasyMed.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DoctorController : BaseController
{
    /// <summary>
    /// Get doctors with a requested specialization
    /// </summary>
    /// <param name="specialization">Medical specialization name</param> 
    /// <returns>Doctors with requested specialization</returns>
    /// <response code="200">Successfully returned doctors</response>
    /// <response code="400">Validation or logic error (e.g. MedicalSpecialization does not exist)</response>
    [Authorize]
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
    [Authorize]
    [HttpGet("freeTerms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FreeTermViewModel>>> GetDoctorFreeTerm(int doctorId,
        DateTime visitDateTime)
    {
        try
        {
            var freeTerms = await Mediator.Send(new GetFreeTermsByDoctorIdQuery(doctorId, visitDateTime));
            return Ok(freeTerms);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get days with a free term for a specific doctor
    /// </summary>
    /// <param name="doctorId">Doctor id</param>
    /// <returns>Days with free term for a specific doctor</returns>
    /// <response code="200">Successfully returned days with free term</response>
    /// <response code="404">Doctor not found</response>
    [Authorize]
    [HttpGet("daysWithFreeTerm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DaysWithFreeTermViewModel>>> GetDaysWithFreeTerm(int doctorId)
    {
        var freeTerms = await Mediator.Send(new GetDaysWithFreeTermQuery(doctorId));
        return Ok(freeTerms);
    }

    /// <summary>
    /// Get medical specializations
    /// </summary>
    /// <returns>Medical specializations</returns>
    /// <response code="200">Successfully returned medical specializations</response>
    [Authorize]
    [HttpGet("specializations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalSpecialization>>> GetMedicalSpecializations()
    {
        var medicalSpecializations = await Mediator.Send(new GetMedicalSpecializationsQuery());
        return Ok(medicalSpecializations);
    }

    /// <summary>
    /// Get doctor information
    /// </summary>
    /// <response code="200">Successfully returned doctor information</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [Authorize]
    [HttpGet("details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorInformation()
    {
        var viewModel = await Mediator.Send(new GetDoctorInformationQuery(RequireUserId()));

        return Ok(viewModel);
    }

    /// <summary>
    /// Update doctor information
    /// </summary>
    /// <response code="200">Successfully updated doctor information</response>
    /// <response code="400">Validation or logic error</response>
    [Authorize]
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateDoctorInformation([FromBody] UpdateDoctorInformationDto dto)
    {
        var viewModel = await Mediator.Send(new UpdateDoctorInformationCommand(RequireUserId(), dto.FirstName,
            dto.LastName, dto.Email, dto.Telephone, dto.Description, dto.OfficeLocation, dto.MedicalSpecialization));

        return Ok(viewModel);
    }

    /// <summary>
    /// Get doctor reviews
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <response code="200">Successfully returned reviews</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [Authorize]
    [HttpGet("{id:int}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorReviews(int id)
    {
        var reviews = await Mediator.Send(new GetReviewsByDoctorIdQuery(id));
        return Ok(reviews);
    }

    /// <summary>
    /// Create review
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <param name="createReviewDto">Description and rating</param>
    /// <response code="200">Successfully created review</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor or patient not found</response>
    [Authorize]
    [HttpPost("{id:int}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateReview(int id, [FromBody] CreateReviewDto createReviewDto)
    {
        var currentUserId = RequireUserId();
        var reviews = await Mediator.Send(
            new CreateReviewCommand(currentUserId, id, createReviewDto.Description, createReviewDto.Rating)
        );
        return Ok(reviews);
    }

    /// <summary>
    /// Get doctor schedule
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <response code="200">Successfully returned doctor's schedule</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [HttpGet("{id:int}/schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDoctorSchedule(int id)
    {
        var schedules = await Mediator.Send(new GetDoctorScheduleQuery(id));
        return Ok(schedules);
    }

    /// <summary>
    /// Get visits
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <param name="isCompleted">Visits filter</param>
    /// <response code="200">Successfully returned visits</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [HttpGet("{id:int}/visits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVisits(int id, [FromQuery] bool? isCompleted)
    {
        var visits = await Mediator.Send(new GetVisitsByDoctorIdQuery(id, isCompleted));
        return Ok(visits);
    }

    /// <summary>
    /// Get prescriptions
    /// </summary>
    /// <param name="id">Doctor id</param>
    /// <response code="200">Successfully returned prescriptions</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="404">Doctor not found</response>
    [HttpGet("{id:int}/prescriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPrescriptions(int id)
    {
        var prescriptions = await Mediator.Send(new GetPrescriptionsByDoctorIdQuery(id));
        return Ok(prescriptions);
    }

    /// <summary>
    /// Create prescription
    /// </summary>
    /// <param name="createPrescriptionDto">Patient Id and list of medicines</param>
    /// <response code="200">Successfully created prescription</response>
    /// <response code="400">Validation or logic error</response>
    /// <response code="403">Unauthorized</response>
    /// <response code="404">Doctor not found</response>
    /// <returns>Created prescription</returns>
    [HttpPost("{id:int}/prescriptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreatePrescription(int id, [FromBody] CreatePrescriptionDto createPrescriptionDto)
    {
        var currentUserId = RequireUserId();
        var medicineViewModelList = createPrescriptionDto.Medicines
            .Select(m => new CreateMedicineViewModel(m.Name, m.Capacity)).ToList();

        var prescriptions =
            await Mediator.Send(new CreatePrescriptionCommand(currentUserId, createPrescriptionDto.PatientId,
                DateOnly.FromDateTime(DateTimeProvider.Now), medicineViewModelList));
        return Ok(prescriptions);
    }
}