using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Represents the outcome of a service operation that can fail with one or more error messages.
/// </summary>
/// <param name="Succeeded">A value indicating whether the operation succeeded.</param>
/// <param name="Errors">The error messages describing why the operation failed, if any.</param>
public record ServiceResult(bool Succeeded, IReadOnlyList<string> Errors)
{
    /// <summary>
    /// Gets a successful <see cref="ServiceResult"/> with no errors.
    /// </summary>
    public static ServiceResult Success { get; } = new(true, Array.Empty<string>());

    /// <summary>
    /// Creates a failed <see cref="ServiceResult"/> with the given error messages.
    /// </summary>
    /// <param name="errors">The error messages describing why the operation failed.</param>
    /// <returns>A failed <see cref="ServiceResult"/>.</returns>
    public static ServiceResult Failure(IEnumerable<string> errors) => new(false, errors.ToList());
}

/// <summary>
/// Defines operations for reading and updating a user's account profile.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Retrieves the profile of a user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose profile is retrieved.</param>
    /// <returns>A task that resolves to the user's profile, or <c>null</c> if the user does not exist.</returns>
    Task<ProfileDto?> GetProfileAsync(string userId);

    /// <summary>
    /// Updates the profile fields of a user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose profile is updated.</param>
    /// <param name="profileDto">The updated profile data.</param>
    /// <returns>A task that resolves to the updated profile, or <c>null</c> if the user does not exist.</returns>
    Task<ProfileDto?> UpdateProfileAsync(string userId, ProfileDto profileDto);

    /// <summary>
    /// Changes the password of a user, verifying the current password first.
    /// </summary>
    /// <param name="userId">The identifier of the user whose password is changed.</param>
    /// <param name="changePasswordRequestDto">The current and new password values.</param>
    /// <returns>A task that resolves to a <see cref="ServiceResult"/> describing the outcome.</returns>
    Task<ServiceResult> ChangePasswordAsync(string userId, ChangePasswordRequestDto changePasswordRequestDto);
}
