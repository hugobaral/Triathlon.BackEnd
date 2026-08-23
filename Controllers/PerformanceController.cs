using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Services;

namespace Triathlon.Api.Controllers;

/// <summary>
/// Exposes aggregated performance chart data for the authenticated user.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PerformanceController : ControllerBase
{
    private readonly IPerformanceService _performanceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceController"/> class.
    /// </summary>
    /// <param name="performanceService">The service used to compute performance summaries.</param>
    public PerformanceController(IPerformanceService performanceService)
    {
        _performanceService = performanceService;
    }

    /// <summary>
    /// Retrieves the current user's performance summary, aggregated per sport by week.
    /// </summary>
    /// <returns>One performance series per sport with recorded activities.</returns>
    [HttpGet("summary")]
    public async Task<ActionResult<IReadOnlyList<PerformanceSeriesDto>>> GetSummary()
    {
        var performanceSummary = await _performanceService.GetPerformanceSummaryAsync(GetCurrentUserId());
        return Ok(performanceSummary);
    }

    /// <summary>
    /// Retrieves the identifier of the currently authenticated user from the JWT claims.
    /// </summary>
    /// <returns>The current user's identifier.</returns>
    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("The current user's identifier claim is missing.");
}
