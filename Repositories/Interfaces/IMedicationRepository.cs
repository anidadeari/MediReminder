using MediReminder.Models;

namespace MediReminder.Repositories.Interfaces;

public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetAllByUserAsync(int userId);
    Task<IEnumerable<Medication>> GetActiveByUserAsync(int userId);
    Task<Medication?> GetByIdAsync(int id, int userId);
    Task AddAsync(Medication medication);
    Task UpdateAsync(Medication medication);
    Task DeleteAsync(Medication medication);
    Task SaveChangesAsync();
}