namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the payload submitted to exchange a refresh token for a new access token.
/// </summary>
/// <param name="RefreshToken">The opaque refresh token previously issued to the client.</param>
public record RefreshRequestDto(string RefreshToken);
