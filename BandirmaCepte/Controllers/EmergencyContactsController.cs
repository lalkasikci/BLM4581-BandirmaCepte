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
        return await _context.EmergencyContacts.ToListAsync();
    }
}
