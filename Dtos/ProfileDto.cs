using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents an athlete's account profile fields.
/// </summary>
/// <param name="Email">The account email address.</param>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
/// <param name="DateOfBirth">The user's date of birth, if provided.</param>
/// <param name="TrainingLevel">The user's self-declared training level.</param>
public record ProfileDto(
    string Email,
    string FirstName,
    string LastName,
    DateOnly? DateOfBirth,
    TrainingLevel TrainingLevel);
