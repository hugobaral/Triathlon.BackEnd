using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the full round-trip shape of a planned training session, used for both
/// create/update requests and read responses.
/// </summary>
/// <param name="Id">The unique identifier of the training session. Ignored on create.</param>
/// <param name="Sport">The sport discipline planned.</param>
/// <param name="ScheduledDate">The calendar date on which the session is scheduled.</param>
/// <param name="ScheduledStartTime">The planned start time of the session, if specified.</param>
/// <param name="DurationMinutes">The planned duration of the session, in minutes.</param>
/// <param name="Notes">Free-form notes about the session, if provided.</param>
/// <param name="IsCompleted">A value indicating whether the session has been completed.</param>
public record TrainingSessionDto(
    int Id,
    SportType Sport,
    DateOnly ScheduledDate,
    TimeOnly? ScheduledStartTime,
    int DurationMinutes,
    string? Notes,
    bool IsCompleted);
