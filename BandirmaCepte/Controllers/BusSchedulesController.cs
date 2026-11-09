using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusSchedulesController : ControllerBase
{
    private readonly BandirmaContext _context;

    public BusSchedulesController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusSchedule>>> GetAll()
    {
        return await _context.BusSchedules
            .Include(s => s.BusLine)
            .OrderBy(s => s.BusLineId)
            .ThenBy(s => s.DepartureTime)
            .ToListAsync();
    }

    [HttpGet("by-line/{busLineId}")]
    public async Task<ActionResult<IEnumerable<BusSchedule>>> GetByBusLine(int busLineId)
    {
        return await _context.BusSchedules
            .Where(s => s.BusLineId == busLineId)
            .OrderBy(s => s.DepartureTime)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BusSchedule>> GetById(int id)
    {
        var schedule = await _context.BusSchedules.FindAsync(id);
        if (schedule == null) return NotFound();
        return schedule;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<BusSchedule>> Create([FromBody] BusSchedule schedule)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var busLineExists = await _context.BusLines.AnyAsync(b => b.Id == schedule.BusLineId);
        if (!busLineExists) return BadRequest("Belirtilen otobüs hattı bulunamadı.");

        _context.BusSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var schedule = await _context.BusSchedules.FindAsync(id);
        if (schedule == null) return NotFound();

        _context.BusSchedules.Remove(schedule);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
