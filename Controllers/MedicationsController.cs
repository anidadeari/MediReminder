using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediReminder.Models;
using MediReminder.Repositories.Interfaces;
using MediReminder.Services.Interfaces;

namespace MediReminder.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationRepository _medicationRepository;
    private readonly IMedicationStatsService _statsService;

    public MedicationsController(
        IMedicationRepository medicationRepository,
        IMedicationStatsService statsService)
    {
        _medicationRepository = medicationRepository;
        _statsService = statsService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medications = await _medicationRepository.GetAllByUserAsync(GetUserId());
        return Ok(medications);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var medications = await _medicationRepository.GetActiveByUserAsync(GetUserId());
        return Ok(medications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var medication = await _medicationRepository.GetByIdAsync(id, GetUserId());
        if (medication == null) return NotFound();
        return Ok(medication);
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetStats(int id)
    {
        try
        {
            var stats = await _statsService.GetStatsAsync(id, GetUserId());
            return Ok(stats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MedicationDto dto)
    {
        var medication = new Medication
        {
            Name = dto.Name,
            Dosage = dto.Dosage,
            Frequency = dto.Frequency,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UserId = GetUserId()
        };

        await _medicationRepository.AddAsync(medication);
        await _medicationRepository.SaveChangesAsync();
        return Ok(medication);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MedicationDto dto)
    {
        var medication = await _medicationRepository.GetByIdAsync(id, GetUserId());
        if (medication == null) return NotFound();

        medication.Name = dto.Name;
        medication.Dosage = dto.Dosage;
        medication.Frequency = dto.Frequency;
        medication.StartDate = dto.StartDate;
        medication.EndDate = dto.EndDate;

        await _medicationRepository.UpdateAsync(medication);
        await _medicationRepository.SaveChangesAsync();
        return Ok(medication);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var medication = await _medicationRepository.GetByIdAsync(id, GetUserId());
        if (medication == null) return NotFound();

        await _medicationRepository.DeleteAsync(medication);
        await _medicationRepository.SaveChangesAsync();
        return Ok("Medication deleted.");
    }
}

public record MedicationDto(
    string Name,
    string Dosage,
    string Frequency,
    DateTime StartDate,
    DateTime? EndDate
);