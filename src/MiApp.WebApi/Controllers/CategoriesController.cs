using MiApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return Ok(categories);
    }
}
