using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediReminder.Repositories.Interfaces;

namespace MediReminder.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/Users - Only Admin can see all users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();

        // Don't expose password hashes
        var result = users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            u.Role,
            u.CreatedAt
        });

        return Ok(result);
    }

    // GET: api/Users/5 - Only Admin can see a specific user
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            user.CreatedAt
        });
    }

    // DELETE: api/Users/5 - Only Admin can delete users
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
        return Ok("User deleted successfully.");
    }
}