using System.Text.Json;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OpenFoodFactsService
{
    private readonly HttpClient _http;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OpenFoodFactsService> _logger;

    public OpenFoodFactsService(HttpClient http, IServiceScopeFactory scopeFactory, ILogger<OpenFoodFactsService> logger)
    {
        _http = http;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task FetchAndStoreAsync(string query = "chocolate", int pageSize = 50)
    {
        var url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={query}&search_simple=1&action=process&json=1&page_size={pageSize}&fields=code,product_name,brands,categories";
        _logger.LogInformation("Fetching from OpenFoodFacts: {Url}", url);

        HttpResponseMessage response;
        try
        {
            response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch from OpenFoodFacts API");
            return;
        }

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        if (!doc.RootElement.TryGetProperty("products", out var productsElement))
            return;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (var item in productsElement.EnumerateArray())
        {
            var code = item.TryGetProperty("code", out var c) ? c.GetString() ?? "" : "";
            if (string.IsNullOrWhiteSpace(code)) continue;

            var exists = await db.Products.AnyAsync(p => p.Code == code);
            if (exists) continue;

            var product = new Product
            {
                Code = code,
                ProductName = item.TryGetProperty("product_name", out var pn) ? pn.GetString() ?? "" : "",
                Brands = item.TryGetProperty("brands", out var b) ? b.GetString() : null,
                Categories = item.TryGetProperty("categories", out var cat) ? cat.GetString() : null
            };

            db.Products.Add(product);
        }

        await db.SaveChangesAsync();
        _logger.LogInformation("ETL complete");
    }
}
