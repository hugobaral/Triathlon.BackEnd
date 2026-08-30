using Microsoft.AspNetCore.Identity;
using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Models.Entities;

/// <summary>
/// Represents an authenticated athlete using the Triathlon training tracker.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's date of birth, if provided.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the user's self-declared training level.
    /// </summary>
    public TrainingLevel TrainingLevel { get; set; } = TrainingLevel.Beginner;

    /// <summary>
    /// Gets or sets the user's weight in kilograms, if provided.
    /// </summary>
    public double? WeightKg { get; set; }

    /// <summary>
    /// Gets or sets the user's height in centimeters, if provided.
    /// </summary>
    public double? HeightCm { get; set; }
}
