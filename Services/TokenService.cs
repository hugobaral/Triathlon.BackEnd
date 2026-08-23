using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Triathlon.Api.Data;
using Triathlon.Api.Models.Entities;

namespace Triathlon.Api.Services;

/// <summary>
/// Issues short-lived JWT access tokens and long-lived opaque refresh tokens, and handles
/// refresh token validation and rotation.
/// </summary>
public class TokenService : ITokenService
{
    private const int AccessTokenLifetimeMinutes = 60;
    private const int RefreshTokenLifetimeDays = 14;

    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to persist refresh tokens.</param>
    /// <param name="configuration">The application configuration containing JWT settings.</param>
    public TokenService(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task<TokenPairResult> GenerateTokenPairAsync(ApplicationUser user)
    {
        var accessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(AccessTokenLifetimeMinutes);
        var accessToken = CreateAccessToken(user, accessTokenExpiresAtUtc);
        var rawRefreshToken = GenerateRawRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(rawRefreshToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(RefreshTokenLifetimeDays)
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new TokenPairResult(accessToken, rawRefreshToken, accessTokenExpiresAtUtc);
    }

    /// <inheritdoc />
    public async Task<TokenPairResult?> RefreshTokenPairAsync(string rawRefreshToken)
    {
        var tokenHash = HashToken(rawRefreshToken);

        var existingToken = await _dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .FirstOrDefaultAsync(refreshToken => refreshToken.TokenHash == tokenHash);

        if (existingToken is null
            || existingToken.RevokedAtUtc is not null
            || existingToken.ExpiresAtUtc <= DateTime.UtcNow
            || existingToken.User is null)
        {
            return null;
        }

        existingToken.RevokedAtUtc = DateTime.UtcNow;

        var newTokenPair = await GenerateTokenPairAsync(existingToken.User);

        await _dbContext.SaveChangesAsync();

        return newTokenPair;
    }

    /// <summary>
    /// Creates a signed JWT access token containing identity claims for the given user.
    /// </summary>
    /// <param name="user">The user to create the token for.</param>
    /// <param name="expiresAtUtc">The UTC timestamp at which the token expires.</param>
    /// <returns>The serialized JWT access token.</returns>
    private string CreateAccessToken(ApplicationUser user, DateTime expiresAtUtc)
    {
        var signingKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Missing required configuration value 'Jwt:Key'.");
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Missing required configuration value 'Jwt:Issuer'.");
        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Missing required configuration value 'Jwt:Audience'.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a cryptographically random raw refresh token.
    /// </summary>
    /// <returns>A URL-safe, base64-encoded random string.</returns>
    private static string GenerateRawRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Computes the SHA-256 hash of a raw token for safe storage.
    /// </summary>
    /// <param name="rawToken">The raw token to hash.</param>
    /// <returns>The base64-encoded SHA-256 hash of the token.</returns>
    private static string HashToken(string rawToken)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToBase64String(hashBytes);
    }
}
