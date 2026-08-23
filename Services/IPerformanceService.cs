using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Defines operations for computing aggregated performance chart data from a user's activities.
/// </summary>
public interface IPerformanceService
{
    /// <summary>
    /// Computes a per-sport performance summary by aggregating a user's activities into weekly buckets.
    /// </summary>
    /// <param name="userId">The identifier of the user whose activities are aggregated.</param>
    /// <returns>A task that resolves to one performance series per sport that has recorded activities.</returns>
    Task<IReadOnlyList<PerformanceSeriesDto>> GetPerformanceSummaryAsync(string userId);
}
