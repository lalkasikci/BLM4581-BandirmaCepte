using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandirmaCepte.Data;
using BandirmaCepte.Models;
using System.Security.Claims;
using BandirmaCepte.Services;

namespace BandirmaCepte.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly BandirmaContext _context;
    private readonly MunicipalNewsService _municipalNews;

    public NewsController(BandirmaContext context, MunicipalNewsService municipalNews)
    {
        _context = context;
        _municipalNews = municipalNews;
    }

    [HttpGet("municipal")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<MunicipalNewsItem>>> GetMunicipalNews(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _municipalNews.GetLatestAsync(cancellationToken));
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { message = "Bandırma Belediyesi haber kaynağına şu anda ulaşılamıyor." });
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Belediye haber kaynağından yanıt zaman aşımına uğradı." });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetAll()
    {
        return await _context.News
            .Include(n => n.User)
            .OrderByDescending(n => n.PublishedAt)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<News>> GetById(int id)
    {
        var item = await _context.News
            .Include(n => n.User)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (item == null) return NotFound();
        return item;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<News>> Create([FromBody] News news)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        news.PublishedAt = DateTime.UtcNow;

        
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            news.UserId = userId;
        }

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
        existing.Source = news.Source;
        existing.IsPublished = news.IsPublished;

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
