using Microsoft.EntityFrameworkCore;
using MediReminder.Data;
using MediReminder.Models;
using MediReminder.Repositories.Interfaces;

namespace MediReminder.Repositories.Implementations;

public class MedicationLogRepository : IMedicationLogRepository
{
    private readonly AppDbContext _context;

    public MedicationLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicationLog>> GetByMedicationIdAsync(int medicationId) =>
        await _context.MedicationLogs
            .Where(l => l.MedicationId == medicationId)
            .OrderByDescending(l => l.TakenAt)
            .ToListAsync();

    public async Task<MedicationLog?> GetByIdAsync(int id) =>
        await _context.MedicationLogs
            .Include(l => l.Medication)
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task AddAsync(MedicationLog log) =>
        await _context.MedicationLogs.AddAsync(log);

    public async Task DeleteAsync(MedicationLog log) =>
        _context.MedicationLogs.Remove(log);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}