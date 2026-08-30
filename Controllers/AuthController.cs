using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Entities;
using Triathlon.Api.Services;
namespace Triathlon.Api.Controllers;

/// <summary>
/// Handles user registration, login, and refresh token exchange.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager.</param>
    /// <param name="signInManager">The ASP.NET Core Identity sign-in manager.</param>
    /// <param name="tokenService">The service used to issue and refresh JWT token pairs.</param>
    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Registers a new user account and issues an initial token pair.
    /// </summary>
    /// <param name="registerRequestDto">The registration payload.</param>
    /// <returns>An <see cref="AuthResponseDto"/> on success, or validation errors on failure.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto registerRequestDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerRequestDto.Email,
            Email = registerRequestDto.Email,
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName
        };

        var createResult = await _userManager.CreateAsync(user, registerRequestDto.Password);

        if (!createResult.Succeeded)
        {
            return BadRequest(createResult.Errors.Select(error => error.Description));
        }

        var tokenPair = await _tokenService.GenerateTokenPairAsync(user);

        return Ok(new AuthResponseDto(tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.AccessTokenExpiresAtUtc));
    }

    /// <summary>
    /// Authenticates a user with an email and password and issues a token pair.
    /// </summary>
    /// <param name="loginRequestDto">The login payload.</param>
    /// <returns>An <see cref="AuthResponseDto"/> on success, or 401 Unauthorized on failure.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var passwordCheckResult = await _signInManager.CheckPasswordSignInAsync(
            user, loginRequestDto.Password, lockoutOnFailure: false);

        if (!passwordCheckResult.Succeeded)
        {
            return Unauthorized("Invalid email or password.");
        }

        var tokenPair = await _tokenService.GenerateTokenPairAsync(user);

        return Ok(new AuthResponseDto(tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.AccessTokenExpiresAtUtc));
    }

    /// <summary>
    /// Exchanges a valid, unexpired refresh token for a new token pair.
    /// </summary>
    /// <param name="refreshRequestDto">The refresh token payload.</param>
    /// <returns>A new <see cref="AuthResponseDto"/> on success, or 401 Unauthorized if the refresh token is invalid.</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshRequestDto refreshRequestDto)
    {
        var tokenPair = await _tokenService.RefreshTokenPairAsync(refreshRequestDto.RefreshToken);

        if (tokenPair is null)
        {
            return Unauthorized("Invalid or expired refresh token.");
        }

        return Ok(new AuthResponseDto(tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.AccessTokenExpiresAtUtc));
    }
}
