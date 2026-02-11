using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Data;
using _2026_spark_backend.Models;

namespace _2026_spark_backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingStatusHistoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingStatusHistoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingStatusHistory>>> GetAll()
    {
        var histories = await _context.BookingStatusHistories
            .AsNoTracking()
            .Include(h => h.BookingStatus)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();

        return Ok(histories);
    }

    [HttpGet("booking/{bookingId:int}")]
    public async Task<ActionResult<IEnumerable<BookingStatusHistory>>> GetByBooking(int bookingId)
    {
        var exists = await _context.Bookings.AnyAsync(b => b.Id == bookingId);
        if (!exists)
        {
            return NotFound();
        }

        var histories = await _context.BookingStatusHistories
            .AsNoTracking()
            .Where(h => h.BookingId == bookingId)
            .Include(h => h.BookingStatus)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();

        return Ok(histories);
    }

    [HttpPost]
    public async Task<ActionResult<BookingStatusHistory>> AddHistory(BookingStatusHistory history)
    {
        var bookingExists = await _context.Bookings.AnyAsync(b => b.Id == history.BookingId);
        var statusExists = await _context.BookingStatuses.AnyAsync(s => s.Id == history.BookingStatusId);

        if (!bookingExists || !statusExists)
        {
            return BadRequest("Booking atau status tidak ditemukan.");
        }

        if (history.ChangedAt == default)
        {
            history.ChangedAt = DateTime.UtcNow;
        }

        _context.BookingStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetByBooking), new { bookingId = history.BookingId }, history);
    }
}
