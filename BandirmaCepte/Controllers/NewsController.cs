using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly BandirmaContext _context;

    public NewsController(BandirmaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetAll()
    {
        return await _context.News.OrderByDescending(n => n.PublishedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<News>> GetById(int id)
    {
        var item = await _context.News.FindAsync(id);
        if (item == null) return NotFound();
        return item;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<News>> Create([FromBody] News news)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        news.PublishedAt = DateTime.UtcNow;
        _context.News.Add(news);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = news.Id }, news);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] News news)
    {
        if (id != news.Id) return BadRequest();

        var existing = await _context.News.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Title = news.Title;
        existing.Content = news.Content;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _context.News.FindAsync(id);
        if (existing == null) return NotFound();

        _context.News.Remove(existing);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
