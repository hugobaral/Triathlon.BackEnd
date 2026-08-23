using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Services;

namespace Triathlon.Api.Controllers;

/// <summary>
/// Manages the authenticated user's planned training sessions.
/// </summary>
[ApiController]
[Route("api/training-schedule")]
[Authorize]
public class TrainingScheduleController : ControllerBase
{
    private readonly ITrainingScheduleService _trainingScheduleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrainingScheduleController"/> class.
    /// </summary>
    /// <param name="trainingScheduleService">The service used to manage training sessions.</param>
    public TrainingScheduleController(ITrainingScheduleService trainingScheduleService)
    {
        _trainingScheduleService = trainingScheduleService;
    }

    /// <summary>
    /// Retrieves the current user's planned training sessions.
    /// </summary>
    /// <returns>The user's training sessions.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TrainingSessionDto>>> GetTrainingSessions()
    {
        var trainingSessions = await _trainingScheduleService.GetTrainingSessionsAsync(GetCurrentUserId());
        return Ok(trainingSessions);
    }

    /// <summary>
    /// Retrieves a single training session belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the training session to retrieve.</param>
    /// <returns>The matching training session, or 404 Not Found.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrainingSessionDto>> GetTrainingSessionById(int id)
    {
        var trainingSession = await _trainingScheduleService.GetTrainingSessionByIdAsync(GetCurrentUserId(), id);
        return trainingSession is null ? NotFound() : Ok(trainingSession);
    }

    /// <summary>
    /// Creates a new training session for the current user.
    /// </summary>
    /// <param name="trainingSessionDto">The training session data to create.</param>
    /// <returns>The created training session.</returns>
    [HttpPost]
    public async Task<ActionResult<TrainingSessionDto>> CreateTrainingSession(TrainingSessionDto trainingSessionDto)
    {
        var createdTrainingSession = await _trainingScheduleService.CreateTrainingSessionAsync(
            GetCurrentUserId(), trainingSessionDto);
        return CreatedAtAction(
            nameof(GetTrainingSessionById), new { id = createdTrainingSession.Id }, createdTrainingSession);
    }

    /// <summary>
    /// Updates an existing training session belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the training session to update.</param>
    /// <param name="trainingSessionDto">The updated training session data.</param>
    /// <returns>The updated training session, or 404 Not Found.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TrainingSessionDto>> UpdateTrainingSession(
        int id, TrainingSessionDto trainingSessionDto)
    {
        var updatedTrainingSession = await _trainingScheduleService.UpdateTrainingSessionAsync(
            GetCurrentUserId(), id, trainingSessionDto);
        return updatedTrainingSession is null ? NotFound() : Ok(updatedTrainingSession);
    }

    /// <summary>
    /// Deletes an existing training session belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the training session to delete.</param>
    /// <returns>204 No Content on success, or 404 Not Found.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrainingSession(int id)
    {
        var wasDeleted = await _trainingScheduleService.DeleteTrainingSessionAsync(GetCurrentUserId(), id);
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
