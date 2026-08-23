using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Services;

namespace Triathlon.Api.Controllers;

/// <summary>
/// Manages the authenticated user's account profile and password.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileController"/> class.
    /// </summary>
    /// <param name="profileService">The service used to read and update the user's profile.</param>
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    /// Retrieves the current user's account profile.
    /// </summary>
    /// <returns>The user's profile, or 404 Not Found.</returns>
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile()
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());
        return profile is null ? NotFound() : Ok(profile);
    }

    /// <summary>
    /// Updates the current user's account profile.
    /// </summary>
    /// <param name="profileDto">The updated profile data.</param>
    /// <returns>The updated profile, or 404 Not Found.</returns>
    [HttpPut]
    public async Task<ActionResult<ProfileDto>> UpdateProfile(ProfileDto profileDto)
    {
        var updatedProfile = await _profileService.UpdateProfileAsync(GetCurrentUserId(), profileDto);
        return updatedProfile is null ? NotFound() : Ok(updatedProfile);
    }

    /// <summary>
    /// Changes the current user's password.
    /// </summary>
    /// <param name="changePasswordRequestDto">The current and new password values.</param>
    /// <returns>204 No Content on success, or 400 Bad Request with error details on failure.</returns>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
    {
        var result = await _profileService.ChangePasswordAsync(GetCurrentUserId(), changePasswordRequestDto);
        return result.Succeeded ? NoContent() : BadRequest(result.Errors);
    }

    /// <summary>
    /// Retrieves the identifier of the currently authenticated user from the JWT claims.
    /// </summary>
    /// <returns>The current user's identifier.</returns>
    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("The current user's identifier claim is missing.");
}
