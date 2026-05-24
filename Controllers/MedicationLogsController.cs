using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediReminder.Models;
using MediReminder.Repositories.Interfaces;

namespace MediReminder.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicationLogsController : ControllerBase
{
    private readonly IMedicationLogRepository _logRepository;
    private readonly IMedicationRepository _medicationRepository;

    public MedicationLogsController(
        IMedicationLogRepository logRepository,
        IMedicationRepository medicationRepository)
    {
        _logRepository = logRepository;
        _medicationRepository = medicationRepository;
    }

    // Helper method to get current user ID from JWT token
    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET: api/MedicationLogs/{medicationId}
    [HttpGet("{medicationId}")]
    public async Task<IActionResult> GetLogs(int medicationId)
    {
        var medication = await _medicationRepository.GetByIdAsync(medicationId, GetUserId());
        if (medication == null) return NotFound("Medication not found.");

        var logs = await _logRepository.GetByMedicationIdAsync(medicationId);

        // Return clean DTOs to avoid circular reference
        var result = logs.Select(l => new
        {
            l.Id,
            l.MedicationId,
            l.WasTaken,
            l.TakenAt,
            l.Notes
        });

        return Ok(result);
    }

    // POST: api/MedicationLogs
    [HttpPost]
    public async Task<IActionResult> AddLog([FromBody] MedicationLogDto dto)
    {
        var medication = await _medicationRepository.GetByIdAsync(dto.MedicationId, GetUserId());
        if (medication == null) return NotFound("Medication not found.");

        var log = new MedicationLog
        {
            MedicationId = dto.MedicationId,
            WasTaken = dto.WasTaken,
            TakenAt = DateTime.UtcNow,
            Notes = dto.Notes
        };

        await _logRepository.AddAsync(log);
        await _logRepository.SaveChangesAsync();

        // Return a clean DTO to avoid circular reference
        return Ok(new
        {
            log.Id,
            log.MedicationId,
            log.WasTaken,
            log.TakenAt,
            log.Notes
        });
    }

    // DELETE: api/MedicationLogs/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _logRepository.GetByIdAsync(id);
        if (log == null || log.Medication.UserId != GetUserId())
            return NotFound("Log not found.");

        await _logRepository.DeleteAsync(log);
        await _logRepository.SaveChangesAsync();
        return Ok("Log deleted successfully.");
    }
}

public record MedicationLogDto(
    int MedicationId,
    bool WasTaken,
    string? Notes
);