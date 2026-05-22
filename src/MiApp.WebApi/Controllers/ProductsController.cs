using MiApp.Application.Interfaces;
using MiApp.Application.UseCases;
using MiApp.Domain.Entities;
using MiApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly GetAllProductsUseCase _getAllProductsUseCase;
    private readonly IProductRepository _productRepository;

    public ProductsController(GetAllProductsUseCase getAllProductsUseCase, IProductRepository productRepository)
    {
        _getAllProductsUseCase = getAllProductsUseCase;
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        // QUERY: pasa por el caso de uso
        var products = await _getAllProductsUseCase.ExecuteAsync(ct);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name, CancellationToken ct)
    {
        var products = await _productRepository.SearchByNameAsync(name, ct);
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        try
        {
            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId);

            await _productRepository.AddAsync(product, ct);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null) return NotFound(new { message = "Producto no encontrado." });

        try
        {
            product.Update(request.Name, request.Description, request.Price, request.Stock);
            await _productRepository.UpdateAsync(product, ct);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null) return NotFound();

        await _productRepository.DeleteAsync(product, ct);
        return NoContent();
    }
}
