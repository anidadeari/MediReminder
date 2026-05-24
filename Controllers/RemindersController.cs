using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediReminder.Services.Interfaces;

namespace MediReminder.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    // Helper method to get current user ID from JWT token
    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET: api/Reminders/next/{medicationId}
    // Returns the next scheduled dose for a specific medication
    [HttpGet("next/{medicationId}")]
    public async Task<IActionResult> GetNextDose(int medicationId)
    {
        try
        {
            var nextDose = await _reminderService.GetNextDoseAsync(medicationId, GetUserId());
            return Ok(nextDose);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/Reminders/adherence/{medicationId}?days=7
    // Returns adherence report for the specified period (default: 7 days)
    [HttpGet("adherence/{medicationId}")]
    public async Task<IActionResult> GetAdherenceReport(int medicationId, [FromQuery] int days = 7)
    {
        if (days <= 0 || days > 365)
            return BadRequest("Days must be between 1 and 365.");

        try
        {
            var report = await _reminderService.GetAdherenceReportAsync(medicationId, GetUserId(), days);
            return Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/Reminders/upcoming
    // Returns upcoming reminders for all active medications of the current user
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingReminders()
    {
        var reminders = await _reminderService.GetUpcomingRemindersAsync(GetUserId());
        return Ok(reminders);
    }
}