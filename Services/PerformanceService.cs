using Microsoft.EntityFrameworkCore;
using Triathlon.Api.Data;
using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Services;

/// <summary>
/// Computes aggregated performance chart data by reading a user's activities and grouping them
/// per sport into weekly buckets, without persisting any separate metrics table.
/// </summary>
public class PerformanceService : IPerformanceService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to read activity data.</param>
    public PerformanceService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PerformanceSeriesDto>> GetPerformanceSummaryAsync(string userId)
    {
        var activities = await _dbContext.Activities
            .Where(activity => activity.UserId == userId)
            .ToListAsync();

        var seriesList = new List<PerformanceSeriesDto>();

        foreach (var sport in Enum.GetValues<SportType>())
        {
            var activitiesForSport = activities.Where(activity => activity.Sport == sport).ToList();

            if (activitiesForSport.Count == 0)
            {
                continue;
            }

            var points = activitiesForSport
                .GroupBy(activity => GetWeekStartDate(activity.ActivityDate))
                .OrderBy(group => group.Key)
                .Select(group => new PerformanceSeriesPointDto(
                    group.Key,
                    group.Sum(activity => activity.DurationMinutes),
                    group.Sum(activity => activity.DistanceKilometers ?? 0),
                    group.Count()))
                .ToList();

            seriesList.Add(new PerformanceSeriesDto(sport, points));
        }

        return seriesList;
    }

    /// <summary>
    /// Computes the Monday that begins the calendar week containing the given date.
    /// </summary>
    /// <param name="date">The date whose containing week is computed.</param>
    /// <returns>The date of the Monday starting that week.</returns>
    private static DateOnly GetWeekStartDate(DateOnly date)
    {
        var daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;
        return date.AddDays(-daysSinceMonday);
    }
}
