using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Data;
using _2026_spark_backend.DTO.Requests;
using _2026_spark_backend.DTO.Responses;
using _2026_spark_backend.Models;

namespace _2026_spark_backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly AppDbContext _context;
    private const int PendingStatusId = 1;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings()
    {
        var bookings = await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Room)
            .Include(b => b.User)
            .Include(b => b.BookingStatus)
            .OrderByDescending(b => b.StartTime)
            .ToListAsync();

        return Ok(bookings.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingResponse>> GetBooking(int id)
    {
        var booking = await GetBookingWithDetails(id);
        if (booking is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(booking));
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> CreateBooking(BookingRequest request)
    {
        var validation = await ValidateRequestAsync(request);
        if (validation is not null)
        {
            return validation;
        }

        var statusId = request.BookingStatusId ?? PendingStatusId;
        var statusExists = await _context.BookingStatuses.AnyAsync(s => s.Id == statusId);
        if (!statusExists)
        {
            return BadRequest("Status booking tidak ditemukan.");
        }

        var booking = new Booking
        {
            RoomId = request.RoomId,
            UserId = request.UserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Purpose = request.Purpose,
            BookingStatusId = statusId
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        _context.BookingStatusHistories.Add(new BookingStatusHistory
        {
            BookingId = booking.Id,
            BookingStatusId = statusId,
            ChangedAt = DateTime.UtcNow,
            Notes = "Status awal"
        });

        await _context.SaveChangesAsync();

        var created = await GetBookingWithDetails(booking.Id);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, MapToResponse(created!));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookingResponse>> UpdateBooking(int id, BookingRequest request)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking is null)
        {
            return NotFound();
        }

        var validation = await ValidateRequestAsync(request);
        if (validation is not null)
        {
            return validation;
        }

        var statusId = request.BookingStatusId ?? booking.BookingStatusId;
        var statusExists = await _context.BookingStatuses.AnyAsync(s => s.Id == statusId);
        if (!statusExists)
        {
            return BadRequest("Status booking tidak ditemukan.");
        }

        var statusChanged = booking.BookingStatusId != statusId;

        booking.RoomId = request.RoomId;
        booking.UserId = request.UserId;
        booking.StartTime = request.StartTime;
        booking.EndTime = request.EndTime;
        booking.Purpose = request.Purpose;
        booking.BookingStatusId = statusId;

        if (statusChanged)
        {
            _context.BookingStatusHistories.Add(new BookingStatusHistory
            {
                BookingId = booking.Id,
                BookingStatusId = statusId,
                ChangedAt = DateTime.UtcNow,
                Notes = "Status diperbarui"
            });
        }

        await _context.SaveChangesAsync();

        var updated = await GetBookingWithDetails(id);
        return Ok(MapToResponse(updated!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking is null)
        {
            return NotFound();
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<ActionResult?> ValidateRequestAsync(BookingRequest request)
    {
        if (request.EndTime <= request.StartTime)
        {
            return BadRequest("Waktu selesai harus lebih besar dari waktu mulai.");
        }

        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == request.RoomId);
        if (!roomExists)
        {
            return NotFound("Ruangan tidak ditemukan.");
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
        if (!userExists)
        {
            return NotFound("Pengguna tidak ditemukan.");
        }

        return null;
    }

    private Task<Booking?> GetBookingWithDetails(int id) => _context.Bookings
        .AsNoTracking()
        .Include(b => b.Room)
        .Include(b => b.User)
        .Include(b => b.BookingStatus)
        .FirstOrDefaultAsync(b => b.Id == id);

    private static BookingResponse MapToResponse(Booking booking) => new()
    {
        Id = booking.Id,
        RoomId = booking.RoomId,
        RoomName = booking.Room?.Name ?? string.Empty,
        UserId = booking.UserId,
        UserName = booking.User?.FullName ?? string.Empty,
        StartTime = booking.StartTime,
        EndTime = booking.EndTime,
        Purpose = booking.Purpose,
        BookingStatusId = booking.BookingStatusId,
        BookingStatusName = booking.BookingStatus?.Name ?? string.Empty
    };
}
