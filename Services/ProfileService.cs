using Microsoft.AspNetCore.Identity;
using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Entities;

namespace Triathlon.Api.Services;

/// <summary>
/// Provides read/update access to a user's account profile and password change functionality,
/// backed by <see cref="UserManager{TUser}"/>.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileService"/> class.
    /// </summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager.</param>
    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<ProfileDto?> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user is null ? null : ToDto(user);
    }

    /// <inheritdoc />
    public async Task<ProfileDto?> UpdateProfileAsync(string userId, ProfileDto profileDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        user.FirstName = profileDto.FirstName;
        user.LastName = profileDto.LastName;
        user.DateOfBirth = profileDto.DateOfBirth;
        user.TrainingLevel = profileDto.TrainingLevel;

        await _userManager.UpdateAsync(user);

        return ToDto(user);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> ChangePasswordAsync(
        string userId, ChangePasswordRequestDto changePasswordRequestDto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return ServiceResult.Failure(new[] { "User not found." });
        }

        var identityResult = await _userManager.ChangePasswordAsync(
            user, changePasswordRequestDto.CurrentPassword, changePasswordRequestDto.NewPassword);

        return identityResult.Succeeded
            ? ServiceResult.Success
            : ServiceResult.Failure(identityResult.Errors.Select(error => error.Description));
    }

    /// <summary>
    /// Maps an <see cref="ApplicationUser"/> entity to its profile DTO representation.
    /// </summary>
    /// <param name="user">The user entity to map.</param>
    /// <returns>The mapped DTO.</returns>
    private static ProfileDto ToDto(ApplicationUser user) => new(
        user.Email ?? string.Empty,
        user.FirstName,
        user.LastName,
        user.DateOfBirth,
        user.TrainingLevel);
}
