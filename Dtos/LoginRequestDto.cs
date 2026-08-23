namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the payload submitted when a user logs in.
/// </summary>
/// <param name="Email">The account email address.</param>
/// <param name="Password">The account password.</param>
public record LoginRequestDto(string Email, string Password);
