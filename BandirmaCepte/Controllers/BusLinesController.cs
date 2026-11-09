using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusLinesController : ControllerBase
{
    private readonly BandirmaContext _context;

    public BusLinesController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusLine>>> GetAll()
    {
        return await _context.BusLines
            .Include(b => b.Schedules)
            .OrderBy(b => b.LineNumber)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BusLine>> GetById(int id)
    {
        var line = await _context.BusLines
            .Include(b => b.Schedules)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (line == null) return NotFound();
        return line;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<BusLine>> Create([FromBody] BusLine line)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.BusLines.Add(line);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = line.Id }, line);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BusLine line)
    {
        if (id != line.Id) return BadRequest();

        var existing = await _context.BusLines.FindAsync(id);
        if (existing == null) return NotFound();

        existing.LineNumber = line.LineNumber;
        existing.Route = line.Route;
        existing.Description = line.Description;
        existing.ScheduleCsv = line.ScheduleCsv;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var line = await _context.BusLines.FindAsync(id);
        if (line == null) return NotFound();

        _context.BusLines.Remove(line);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
