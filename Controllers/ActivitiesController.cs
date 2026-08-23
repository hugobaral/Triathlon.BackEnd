using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Enums;
using Triathlon.Api.Services;

namespace Triathlon.Api.Controllers;

/// <summary>
/// Manages the authenticated user's completed training activities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivitiesController"/> class.
    /// </summary>
    /// <param name="activityService">The service used to manage activities.</param>
    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    /// <summary>
    /// Retrieves the current user's activities, optionally filtered by sport and date range.
    /// </summary>
    /// <param name="sport">An optional sport to filter by.</param>
    /// <param name="startDate">An optional inclusive lower bound on the activity date.</param>
    /// <param name="endDate">An optional inclusive upper bound on the activity date.</param>
    /// <returns>The matching activities.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ActivityDto>>> GetActivities(
        [FromQuery] SportType? sport, [FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate)
    {
        var activities = await _activityService.GetActivitiesAsync(GetCurrentUserId(), sport, startDate, endDate);
        return Ok(activities);
    }

    /// <summary>
    /// Retrieves a single activity belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the activity to retrieve.</param>
    /// <returns>The matching activity, or 404 Not Found.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivityDto>> GetActivityById(int id)
    {
        var activity = await _activityService.GetActivityByIdAsync(GetCurrentUserId(), id);
        return activity is null ? NotFound() : Ok(activity);
    }

    /// <summary>
    /// Creates a new activity for the current user.
    /// </summary>
    /// <param name="activityDto">The activity data to create.</param>
    /// <returns>The created activity.</returns>
    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(ActivityDto activityDto)
    {
        var createdActivity = await _activityService.CreateActivityAsync(GetCurrentUserId(), activityDto);
        return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.Id }, createdActivity);
    }

    /// <summary>
    /// Updates an existing activity belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the activity to update.</param>
    /// <param name="activityDto">The updated activity data.</param>
    /// <returns>The updated activity, or 404 Not Found.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ActivityDto>> UpdateActivity(int id, ActivityDto activityDto)
    {
        var updatedActivity = await _activityService.UpdateActivityAsync(GetCurrentUserId(), id, activityDto);
        return updatedActivity is null ? NotFound() : Ok(updatedActivity);
    }

    /// <summary>
    /// Deletes an existing activity belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the activity to delete.</param>
    /// <returns>204 No Content on success, or 404 Not Found.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var wasDeleted = await _activityService.DeleteActivityAsync(GetCurrentUserId(), id);
        return wasDeleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves the identifier of the currently authenticated user from the JWT claims.
    /// </summary>
    /// <returns>The current user's identifier.</returns>
    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("The current user's identifier claim is missing.");
}
