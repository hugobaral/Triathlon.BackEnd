using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the full round-trip shape of a completed training activity, used for both
/// create/update requests and read responses.
/// </summary>
/// <param name="Id">The unique identifier of the activity. Ignored on create.</param>
/// <param name="Sport">The sport discipline performed.</param>
/// <param name="ActivityDate">The calendar date on which the activity took place.</param>
/// <param name="DurationMinutes">The duration of the activity, in minutes.</param>
/// <param name="DistanceKilometers">The distance covered, in kilometers, if applicable.</param>
/// <param name="Notes">Free-form notes about the activity, if provided.</param>
/// <param name="CreatedAtUtc">The UTC timestamp at which the activity record was created. Ignored on create.</param>
public record ActivityDto(
    int Id,
    SportType Sport,
    DateOnly ActivityDate,
    int DurationMinutes,
    double? DistanceKilometers,
    string? Notes,
    DateTime CreatedAtUtc);
