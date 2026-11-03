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
        return await _context.BusLines.ToListAsync();
    }
}
