using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents a single point in a performance time series, aggregated over a period such as a week.
/// </summary>
/// <param name="PeriodStartDate">The start date of the aggregation period this point represents.</param>
/// <param name="TotalDurationMinutes">The total duration, in minutes, of activities within the period.</param>
/// <param name="TotalDistanceKilometers">The total distance, in kilometers, of activities within the period.</param>
/// <param name="ActivityCount">The number of activities recorded within the period.</param>
public record PerformanceSeriesPointDto(
    DateOnly PeriodStartDate,
    int TotalDurationMinutes,
    double TotalDistanceKilometers,
    int ActivityCount);

/// <summary>
/// Represents an aggregated performance series for a single sport, used to render a performance chart.
/// </summary>
/// <param name="Sport">The sport discipline this series represents.</param>
/// <param name="Points">The ordered list of aggregated date/value points.</param>
public record PerformanceSeriesDto(SportType Sport, IReadOnlyList<PerformanceSeriesPointDto> Points);
