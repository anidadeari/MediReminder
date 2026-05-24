namespace MediReminder.Models;

public class MedicationLog
{
    public int Id { get; set; }
    public DateTime TakenAt { get; set; } = DateTime.UtcNow;
    public bool WasTaken { get; set; }
    public string? Notes { get; set; }

    public int MedicationId { get; set; }
    public Medication Medication { get; set; } = null!;
}