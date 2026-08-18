using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly OpenFoodFactsService _etl;

    public ProductsController(AppDbContext db, OpenFoodFactsService etl)
    {
        _db = db;
        _etl = etl;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? search)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                     (p.Brands != null && p.Brands.ToLower().Contains(search.ToLower())));

        var products = await query.Take(100).ToListAsync();
        return Ok(products);
    }

    [HttpPost("fetch")]
    public async Task<IActionResult> Fetch([FromQuery] string query = "chocolate")
    {
        await _etl.FetchAndStoreAsync(query);
        return Ok(new { message = "ETL complete" });
    }
}
