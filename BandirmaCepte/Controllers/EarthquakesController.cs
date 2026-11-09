using Microsoft.AspNetCore.Authorization;
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
        return await _context.Earthquakes
            .OrderByDescending(e => e.OccurrenceTime)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Earthquake>> GetById(int id)
    {
        var earthquake = await _context.Earthquakes.FindAsync(id);
        if (earthquake == null) return NotFound();
        return earthquake;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<Earthquake>> Create([FromBody] Earthquake earthquake)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (earthquake.OccurrenceTime == default)
            earthquake.OccurrenceTime = DateTime.UtcNow;

        _context.Earthquakes.Add(earthquake);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = earthquake.Id }, earthquake);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _context.Earthquakes.FindAsync(id);
        if (existing == null) return NotFound();

        _context.Earthquakes.Remove(existing);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
