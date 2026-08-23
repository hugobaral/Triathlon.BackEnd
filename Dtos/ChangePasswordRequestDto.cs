namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the payload submitted when a user changes their password.
/// </summary>
/// <param name="CurrentPassword">The user's current password.</param>
/// <param name="NewPassword">The desired new password.</param>
public record ChangePasswordRequestDto(string CurrentPassword, string NewPassword);
