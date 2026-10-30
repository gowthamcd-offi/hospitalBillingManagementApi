using hospitalBillingManagement.DTOs;
using hospitalBillingManagement.Models;
using hospitalBillingManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hospitalBillingManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<Product>> GetById(int productId)
    {
        var product = await _productService.GetByIdAsync(productId);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("multiple")]
    public async Task<ActionResult<Product[]>> GetByIds([FromQuery] int[] productIds)
    {
        var products = await _productService.GetByIdAsync(productIds);
        return Ok(products);
    }
}
