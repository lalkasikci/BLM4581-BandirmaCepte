using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PharmaciesController : ControllerBase
{
    private readonly BandirmaContext _context;

    public PharmaciesController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pharmacy>>> GetAll()
    {
        return await _context.Pharmacies.ToListAsync();
    }
}
