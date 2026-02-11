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
public class RoomController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoomController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponse>>> GetRooms()
    {
        var rooms = await _context.Rooms
            .AsNoTracking()
            .Select(r => MapToResponse(r))
            .ToListAsync();

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse>> GetRoom(int id)
    {
        var room = await _context.Rooms.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(room));
    }

    [HttpPost]
    public async Task<ActionResult<RoomResponse>> CreateRoom(RoomRequest request)
    {
        var room = new Room
        {
            Name = request.Name,
            Location = request.Location,
            Capacity = request.Capacity,
            Description = request.Description
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, MapToResponse(room));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse>> UpdateRoom(int id, RoomRequest request)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null)
        {
            return NotFound();
        }

        room.Name = request.Name;
        room.Location = request.Location;
        room.Capacity = request.Capacity;
        room.Description = request.Description;

        await _context.SaveChangesAsync();

        return Ok(MapToResponse(room));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null)
        {
            return NotFound();
        }

        var hasBooking = await _context.Bookings.AnyAsync(b => b.RoomId == id);
        if (hasBooking)
        {
            return BadRequest("Tidak dapat menghapus ruangan yang masih memiliki peminjaman.");
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static RoomResponse MapToResponse(Room room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        Location = room.Location,
        Capacity = room.Capacity,
        Description = room.Description
    };
}
