using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_spark_backend.Data;
using _2026_spark_backend.DTO.Requests;
using _2026_spark_backend.DTO.Responses;
using _2026_spark_backend.Models;
using _2026_spark_backend.Services;

namespace _2026_spark_backend.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;

    public UserController(AppDbContext context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Select(u => new UserResponse
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser(UserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Password wajib diisi.");
        }

        var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (emailExists)
        {
            return Conflict("Email sudah terdaftar.");
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            PasswordHash = _passwordService.Hash(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = MapToResponse(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(int id, UserRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email && u.Id != id);
        if (emailExists)
        {
            return Conflict("Email sudah digunakan oleh pengguna lain.");
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Role = request.Role;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = _passwordService.Hash(request.Password);
        }

        await _context.SaveChangesAsync();

        return Ok(MapToResponse(user));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var hasBookings = await _context.Bookings.AnyAsync(b => b.UserId == id);
        if (hasBookings)
        {
            return BadRequest("Tidak dapat menghapus pengguna yang memiliki data peminjaman.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static UserResponse MapToResponse(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role
    };
}
