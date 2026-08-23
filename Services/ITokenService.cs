using Triathlon.Api.Models.Entities;

namespace Triathlon.Api.Services;

/// <summary>
/// Represents the result of issuing a new token pair to a user.
/// </summary>
/// <param name="AccessToken">The short-lived JWT access token.</param>
/// <param name="RefreshToken">The long-lived opaque refresh token, in raw (unhashed) form.</param>
/// <param name="AccessTokenExpiresAtUtc">The UTC timestamp at which the access token expires.</param>
public record TokenPairResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);

/// <summary>
/// Defines operations for issuing and validating JWT access tokens and opaque refresh tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a new JWT access token and a new opaque refresh token for the given user,
    /// persisting a hash of the refresh token.
    /// </summary>
    /// <param name="user">The user to issue tokens for.</param>
    /// <returns>A task that resolves to the issued token pair.</returns>
    Task<TokenPairResult> GenerateTokenPairAsync(ApplicationUser user);

    /// <summary>
    /// Validates a raw refresh token against the stored hash, revokes it, and issues a new
    /// token pair if it is valid and not expired or revoked.
    /// </summary>
    /// <param name="rawRefreshToken">The raw refresh token presented by the client.</param>
    /// <returns>A task that resolves to the newly issued token pair, or <c>null</c> if the refresh token is invalid.</returns>
    Task<TokenPairResult?> RefreshTokenPairAsync(string rawRefreshToken);
}
