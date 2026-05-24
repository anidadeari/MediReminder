using MediReminder.Models;

namespace MediReminder.Repositories.Interfaces;

public interface IMedicationLogRepository
{
    Task<IEnumerable<MedicationLog>> GetByMedicationIdAsync(int medicationId);
    Task<MedicationLog?> GetByIdAsync(int id);
    Task AddAsync(MedicationLog log);
    Task DeleteAsync(MedicationLog log);
    Task SaveChangesAsync();
}