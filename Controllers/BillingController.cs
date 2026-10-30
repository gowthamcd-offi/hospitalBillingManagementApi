using hospitalBillingManagement.DTOs;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hospitalBillingManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;

    public BillingController(IBillingService billingService)
    {
        _billingService = billingService;
    }

    [HttpGet("{identifier}")]
    public async Task<ActionResult<Patient>> GetBillDetails(string identifier)
    {
        var patient = await _billingService.GetBillDetailsAsync(identifier);
        if (patient == null) return NotFound();
        return Ok(patient);
    }

    [HttpPost("{identifier}/pay")]
    public async Task<ActionResult<bool>> SettleBill(string identifier, [FromBody] BillingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PaymentDetail))
            return BadRequest(new { message = "Payment detail is required" });

        var success = await _billingService.SettleBillAsync(identifier, request.PaymentDetail);
        
        if (!success)
        {
            return NotFound(new { message = "Patient not found or payment failed" });
        }
        
        return Ok(new { message = "Payment successful", paid = true });
    }
}
