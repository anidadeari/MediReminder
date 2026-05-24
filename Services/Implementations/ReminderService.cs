using MediReminder.Models;
using MediReminder.Repositories.Interfaces;
using MediReminder.Services.Interfaces;

namespace MediReminder.Services.Implementations;

public class ReminderService : IReminderService
{
    private readonly IMedicationRepository _medicationRepository;
    private readonly IMedicationLogRepository _logRepository;

    public ReminderService(
        IMedicationRepository medicationRepository,
        IMedicationLogRepository logRepository)
    {
        _medicationRepository = medicationRepository;
        _logRepository = logRepository;
    }

    // 1. Calculate the next dose time for a specific medication
    public async Task<NextDoseDto> GetNextDoseAsync(int medicationId, int userId)
    {
        var medication = await _medicationRepository.GetByIdAsync(medicationId, userId);
        if (medication == null)
            throw new KeyNotFoundException("Medication not found.");

        // Get the most recent log entry
        var logs = await _logRepository.GetByMedicationIdAsync(medicationId);
        var lastLog = logs.OrderByDescending(l => l.TakenAt).FirstOrDefault();

        // Calculate next dose time based on Frequency
        int hoursInterval = GetHoursFromFrequency(medication.Frequency);

        DateTime nextDoseTime;
        if (lastLog != null)
        {
            nextDoseTime = lastLog.TakenAt.AddHours(hoursInterval);
        }
        else
        {
            // If no logs exist, start from medication's StartDate
            nextDoseTime = medication.StartDate;
        }

        var now = DateTime.UtcNow;
        var hoursUntilNext = (int)(nextDoseTime - now).TotalHours;
        bool isOverdue = nextDoseTime < now;

        return new NextDoseDto(
            medication.Id,
            medication.Name,
            nextDoseTime,
            medication.Frequency,
            hoursUntilNext,
            isOverdue
        );
    }

    // 2. Generate an adherence report for the last X days
    public async Task<AdherenceReportDto> GetAdherenceReportAsync(int medicationId, int userId, int days)
    {
        var medication = await _medicationRepository.GetByIdAsync(medicationId, userId);
        if (medication == null)
            throw new KeyNotFoundException("Medication not found.");

        var toDate = DateTime.UtcNow;
        var fromDate = toDate.AddDays(-days);

        // Retrieve all logs within the specified period
        var logs = await _logRepository.GetByMedicationIdAsync(medicationId);
        var logsInPeriod = logs.Where(l => l.TakenAt >= fromDate && l.TakenAt <= toDate).ToList();

        // Calculate expected number of doses based on frequency
        int hoursInterval = GetHoursFromFrequency(medication.Frequency);
        int totalHours = days * 24;
        int expectedDoses = totalHours / hoursInterval;

        int takenDoses = logsInPeriod.Count(l => l.WasTaken);
        int missedDoses = Math.Max(0, expectedDoses - takenDoses);

        double adherencePercentage = expectedDoses > 0
            ? Math.Round((double)takenDoses / expectedDoses * 100, 2)
            : 0;

        // Categorize adherence status
        string status = adherencePercentage switch
        {
            >= 90 => "Excellent",
            >= 70 => "Good",
            _ => "Poor"
        };

        return new AdherenceReportDto(
            medication.Id,
            medication.Name,
            days,
            fromDate,
            toDate,
            expectedDoses,
            takenDoses,
            missedDoses,
            adherencePercentage,
            status
        );
    }

    // 3. Get upcoming reminders for all active medications
    public async Task<IEnumerable<UpcomingReminderDto>> GetUpcomingRemindersAsync(int userId)
    {
        var activeMedications = await _medicationRepository.GetActiveByUserAsync(userId);
        var reminders = new List<UpcomingReminderDto>();

        foreach (var medication in activeMedications)
        {
            var logs = await _logRepository.GetByMedicationIdAsync(medication.Id);
            var lastLog = logs.OrderByDescending(l => l.TakenAt).FirstOrDefault();

            int hoursInterval = GetHoursFromFrequency(medication.Frequency);
            DateTime nextDoseTime = lastLog != null
                ? lastLog.TakenAt.AddHours(hoursInterval)
                : medication.StartDate;

            int hoursUntilNext = (int)(nextDoseTime - DateTime.UtcNow).TotalHours;

            reminders.Add(new UpcomingReminderDto(
                medication.Id,
                medication.Name,
                medication.Dosage,
                nextDoseTime,
                hoursUntilNext
            ));
        }

        // Sort reminders by the soonest upcoming dose
        return reminders.OrderBy(r => r.NextDoseTime);
    }

    // Helper method - converts Frequency string into hours interval
    private int GetHoursFromFrequency(string frequency)
    {
        return frequency.ToLower() switch
        {
            "daily" or "once a day" or "1x daily" => 24,
            "twice a day" or "2x daily" or "every 12 hours" => 12,
            "three times a day" or "3x daily" or "every 8 hours" => 8,
            "four times a day" or "4x daily" or "every 6 hours" => 6,
            "every 4 hours" => 4,
            "weekly" => 168,
            _ => 24  // default: daily
        };
    }
}