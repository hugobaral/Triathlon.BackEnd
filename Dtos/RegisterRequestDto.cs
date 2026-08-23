namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the payload submitted when a new user registers an account.
/// </summary>
/// <param name="Email">The email address to register with.</param>
/// <param name="Password">The desired account password.</param>
/// <param name="FirstName">The user's first name.</param>
/// <param name="LastName">The user's last name.</param>
public record RegisterRequestDto(string Email, string Password, string FirstName, string LastName);
