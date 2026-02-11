using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Data;
using _2026_spark_backend.Models;

namespace _2026_spark_backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingStatusController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingStatusController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingStatus>>> GetStatuses()
    {
        var statuses = await _context.BookingStatuses.AsNoTracking().ToListAsync();
        return Ok(statuses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingStatus>> GetStatus(int id)
    {
        var status = await _context.BookingStatuses.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (status is null)
        {
            return NotFound();
        }

        return Ok(status);
    }

    [HttpPost]
    public async Task<ActionResult<BookingStatus>> CreateStatus(BookingStatus status)
    {
        var nameExists = await _context.BookingStatuses.AnyAsync(s => s.Name == status.Name);
        if (nameExists)
        {
            return Conflict("Status dengan nama sama sudah ada.");
        }

        _context.BookingStatuses.Add(status);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStatus), new { id = status.Id }, status);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookingStatus>> UpdateStatus(int id, BookingStatus status)
    {
        var existing = await _context.BookingStatuses.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        var nameExists = await _context.BookingStatuses.AnyAsync(s => s.Name == status.Name && s.Id != id);
        if (nameExists)
        {
            return Conflict("Status dengan nama sama sudah ada.");
        }

        existing.Name = status.Name;
        existing.Description = status.Description;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        var status = await _context.BookingStatuses.FindAsync(id);
        if (status is null)
        {
            return NotFound();
        }

        var isInUse = await _context.Bookings.AnyAsync(b => b.BookingStatusId == id)
                       || await _context.BookingStatusHistories.AnyAsync(h => h.BookingStatusId == id);
        if (isInUse)
        {
            return BadRequest("Status sedang digunakan pada data peminjaman.");
        }

        _context.BookingStatuses.Remove(status);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
