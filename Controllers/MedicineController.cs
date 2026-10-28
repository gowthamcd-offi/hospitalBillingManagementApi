using hospitalBillingManagement.DTOs;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hospitalBillingManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
    private readonly IPatientService _patientService;

    public MedicineController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("{identifier}/add")]
    public async Task<ActionResult<bool>> AddMedicineAsync(string identifier, [FromBody] AddMedicineRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero");

        var success = await _patientService.AddMedicineAsync(identifier, request.ProductId, request.Quantity);
        if (!success) return NotFound("Patient or product not found");
        return Ok(success);
    }
}
