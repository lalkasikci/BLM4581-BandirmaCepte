using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EarthquakesController : ControllerBase
{
    private readonly BandirmaContext _context;

    public EarthquakesController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Earthquake>>> GetAll()
    {
        return await _context.Earthquakes.OrderByDescending(e => e.OccurrenceTime).ToListAsync();
    }
}
