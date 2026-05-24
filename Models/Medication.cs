namespace MediReminder.Models;

public class Medication
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<MedicationLog> Logs { get; set; } = new List<MedicationLog>();
}