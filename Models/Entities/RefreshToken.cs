namespace Triathlon.Api.Models.Entities;

/// <summary>
/// Represents a hashed refresh token issued to a user as part of the JWT refresh flow.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Gets or sets the unique identifier of the refresh token record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns this refresh token.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the navigation property to the owning user.
    /// </summary>
    public ApplicationUser? User { get; set; }

    /// <summary>
    /// Gets or sets the SHA-256 hash of the raw refresh token. The raw token is never stored.
    /// </summary>
    public string TokenHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC timestamp at which the refresh token expires.
    /// </summary>
    public DateTime ExpiresAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp at which the refresh token was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp at which the refresh token was revoked, if applicable.
    /// </summary>
    public DateTime? RevokedAtUtc { get; set; }
}
