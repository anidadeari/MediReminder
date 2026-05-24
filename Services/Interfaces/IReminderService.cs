namespace MediReminder.Services.Interfaces;

public interface IReminderService
{
    Task<NextDoseDto> GetNextDoseAsync(int medicationId, int userId);
    Task<AdherenceReportDto> GetAdherenceReportAsync(int medicationId, int userId, int days);
    Task<IEnumerable<UpcomingReminderDto>> GetUpcomingRemindersAsync(int userId);
}

// DTO për dozën e ardhshme
public record NextDoseDto(
    int MedicationId,
    string MedicationName,
    DateTime NextDoseTime,
    string Frequency,
    int HoursUntilNext,
    bool IsOverdue
);

// DTO për raportin e adherence
public record AdherenceReportDto(
    int MedicationId,
    string MedicationName,
    int Days,
    DateTime FromDate,
    DateTime ToDate,
    int ExpectedDoses,
    int TakenDoses,
    int MissedDoses,
    double AdherencePercentage,
    string Status  // "Excellent", "Good", "Poor"
);

// DTO për lista e reminderave të ardhshme
public record UpcomingReminderDto(
    int MedicationId,
    string MedicationName,
    string Dosage,
    DateTime NextDoseTime,
    int HoursUntilNext
);