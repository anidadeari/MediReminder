using Microsoft.EntityFrameworkCore;
using MediReminder.Data;
using MediReminder.Models;
using MediReminder.Repositories.Interfaces;

namespace MediReminder.Repositories.Implementations;

public class MedicationRepository : IMedicationRepository
{
    private readonly AppDbContext _context;

    public MedicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Medication>> GetAllByUserAsync(int userId) =>
        await _context.Medications
            .Where(m => m.UserId == userId)
            .ToListAsync();

    public async Task<IEnumerable<Medication>> GetActiveByUserAsync(int userId) =>
        await _context.Medications
            .Where(m => m.UserId == userId && m.IsActive)
            .ToListAsync();

    public async Task<Medication?> GetByIdAsync(int id, int userId) =>
        await _context.Medications
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

    public async Task AddAsync(Medication medication) =>
        await _context.Medications.AddAsync(medication);

    public async Task UpdateAsync(Medication medication) =>
        _context.Medications.Update(medication);

    public async Task DeleteAsync(Medication medication) =>
        _context.Medications.Remove(medication);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}