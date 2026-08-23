namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the tokens returned after a successful authentication or refresh operation.
/// </summary>
/// <param name="AccessToken">The short-lived JWT access token.</param>
/// <param name="RefreshToken">The long-lived opaque refresh token.</param>
/// <param name="ExpiresAtUtc">The UTC timestamp at which the access token expires.</param>
public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
