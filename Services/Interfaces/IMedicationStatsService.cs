namespace MediReminder.Services.Interfaces;

public interface IMedicationStatsService
{
    Task<MedicationStatsDto> GetStatsAsync(int medicationId, int userId);
}

public record MedicationStatsDto(
    int MedicationId,
    string MedicationName,
    int TotalLogs,
    int TakenCount,
    int MissedCount,
    double SuccessRate
);