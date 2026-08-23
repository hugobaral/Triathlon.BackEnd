using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Models.Entities;

/// <summary>
/// Represents a completed training session logged after the fact by an athlete.
/// </summary>
public class Activity
{
    /// <summary>
    /// Gets or sets the unique identifier of the activity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns this activity.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the navigation property to the owning user.
    /// </summary>
    public ApplicationUser? User { get; set; }

    /// <summary>
    /// Gets or sets the sport discipline performed.
    /// </summary>
    public SportType Sport { get; set; }

    /// <summary>
    /// Gets or sets the calendar date on which the activity took place.
    /// </summary>
    public DateOnly ActivityDate { get; set; }

    /// <summary>
    /// Gets or sets the duration of the activity, in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets the distance covered, in kilometers, if applicable.
    /// </summary>
    public double? DistanceKilometers { get; set; }

    /// <summary>
    /// Gets or sets free-form notes about the activity.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp at which the activity record was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }
}
