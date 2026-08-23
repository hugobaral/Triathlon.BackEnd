using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Models.Entities;

/// <summary>
/// Represents a planned, editable training session on an athlete's calendar.
/// </summary>
public class TrainingSession
{
    /// <summary>
    /// Gets or sets the unique identifier of the training session.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns this training session.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the navigation property to the owning user.
    /// </summary>
    public ApplicationUser? User { get; set; }

    /// <summary>
    /// Gets or sets the sport discipline planned.
    /// </summary>
    public SportType Sport { get; set; }

    /// <summary>
    /// Gets or sets the calendar date on which the session is scheduled.
    /// </summary>
    public DateOnly ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the planned start time of the session, if specified.
    /// </summary>
    public TimeOnly? ScheduledStartTime { get; set; }

    /// <summary>
    /// Gets or sets the planned duration of the session, in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets free-form notes about the session.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the session has been completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}
