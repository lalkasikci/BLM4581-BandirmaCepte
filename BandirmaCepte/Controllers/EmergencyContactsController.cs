using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmergencyContactsController : ControllerBase
{
    private readonly BandirmaContext _context;

    public EmergencyContactsController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmergencyContact>>> GetAll()
    {
        return await _context.EmergencyContacts.OrderBy(c => c.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmergencyContact>> GetById(int id)
    {
        var contact = await _context.EmergencyContacts.FindAsync(id);
        if (contact == null) return NotFound();
        return contact;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<EmergencyContact>> Create([FromBody] EmergencyContact contact)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.EmergencyContacts.Add(contact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmergencyContact contact)
    {
        if (id != contact.Id) return BadRequest();

        var existing = await _context.EmergencyContacts.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Name = contact.Name;
        existing.PhoneNumber = contact.PhoneNumber;
        existing.Category = contact.Category;
        existing.Description = contact.Description;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _context.EmergencyContacts.FindAsync(id);
        if (existing == null) return NotFound();

        _context.EmergencyContacts.Remove(existing);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
