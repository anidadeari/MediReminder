using Microsoft.EntityFrameworkCore;
using MediReminder.Models;

namespace MediReminder.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<MedicationLog> MedicationLogs { get; set; }
}