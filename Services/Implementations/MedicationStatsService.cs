using MediReminder.Repositories.Interfaces;
using MediReminder.Services.Interfaces;

namespace MediReminder.Services.Implementations;

public class MedicationStatsService : IMedicationStatsService
{
    private readonly IMedicationRepository _medicationRepository;
    private readonly IMedicationLogRepository _logRepository;

    public MedicationStatsService(
        IMedicationRepository medicationRepository,
        IMedicationLogRepository logRepository)
    {
        _medicationRepository = medicationRepository;
        _logRepository = logRepository;
    }

    public async Task<MedicationStatsDto> GetStatsAsync(int medicationId, int userId)
    {
        var medication = await _medicationRepository.GetByIdAsync(medicationId, userId);
        if (medication == null)
            throw new KeyNotFoundException("Medication not found.");

        var logs = await _logRepository.GetByMedicationIdAsync(medicationId);
        var logList = logs.ToList();

        int total = logList.Count;
        int taken = logList.Count(l => l.WasTaken);
        int missed = total - taken;
        double successRate = total > 0 ? Math.Round((double)taken / total * 100, 2) : 0;

        return new MedicationStatsDto(
            medicationId,
            medication.Name,
            total,
            taken,
            missed,
            successRate
        );
    }
}