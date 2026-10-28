using hospitalBillingManagement.DTOs;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hospitalBillingManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreateAsync([FromBody] CreatePatientRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var patient = await _patientService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.MobileNo
        );

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAllAsync()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(string id)
    {
        var patient = await _patientService.SearchAsync(id);
        if (patient == null) return NotFound();
        return Ok(patient);
    }

    [HttpGet("search")]
    public async Task<ActionResult<Patient>> SearchAsync([FromQuery] string identifier)
    {
        var patient = await _patientService.SearchAsync(identifier);
        if (patient == null) return NotFound();
        return Ok(patient);
    }

    [HttpGet("by-product/{productId}")]
    public async Task<ActionResult<Patient[]>> GetPatientsByProductAsync(int productId)
    {
        var patients = await _patientService.GetPatientsByProductAsync(productId);
        return Ok(patients);
    }
}
