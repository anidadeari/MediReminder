using Microsoft.EntityFrameworkCore;
using MediReminder.Data;
using MediReminder.Models;
using MediReminder.Repositories.Interfaces;

namespace MediReminder.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public async Task DeleteAsync(User user) =>
        _context.Users.Remove(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}