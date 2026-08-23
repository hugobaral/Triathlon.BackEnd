using Microsoft.EntityFrameworkCore;
using Triathlon.Api.Data;
using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Services;

/// <summary>
/// Provides CRUD operations over a user's completed training activities, always scoped to
/// the authenticated user's identifier.
/// </summary>
public class ActivityService : IActivityService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to access activity data.</param>
    public ActivityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ActivityDto>> GetActivitiesAsync(
        string userId, SportType? sport, DateOnly? startDate, DateOnly? endDate)
    {
        var query = _dbContext.Activities.Where(activity => activity.UserId == userId);

        if (sport is not null)
        {
            query = query.Where(activity => activity.Sport == sport);
        }

        if (startDate is not null)
        {
            query = query.Where(activity => activity.ActivityDate >= startDate);
        }

        if (endDate is not null)
        {
            query = query.Where(activity => activity.ActivityDate <= endDate);
        }

        var activities = await query
            .OrderByDescending(activity => activity.ActivityDate)
            .ToListAsync();

        return activities.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<ActivityDto?> GetActivityByIdAsync(string userId, int activityId)
    {
        var activity = await _dbContext.Activities
            .FirstOrDefaultAsync(activity => activity.UserId == userId && activity.Id == activityId);

        return activity is null ? null : ToDto(activity);
    }

    /// <inheritdoc />
    public async Task<ActivityDto> CreateActivityAsync(string userId, ActivityDto activityDto)
    {
        var activity = new Models.Entities.Activity
        {
            UserId = userId,
            Sport = activityDto.Sport,
            ActivityDate = activityDto.ActivityDate,
            DurationMinutes = activityDto.DurationMinutes,
            DistanceKilometers = activityDto.DistanceKilometers,
            Notes = activityDto.Notes,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Activities.Add(activity);
        await _dbContext.SaveChangesAsync();

        return ToDto(activity);
    }

    /// <inheritdoc />
    public async Task<ActivityDto?> UpdateActivityAsync(string userId, int activityId, ActivityDto activityDto)
    {
        var activity = await _dbContext.Activities
            .FirstOrDefaultAsync(activity => activity.UserId == userId && activity.Id == activityId);

        if (activity is null)
        {
            return null;
        }

        activity.Sport = activityDto.Sport;
        activity.ActivityDate = activityDto.ActivityDate;
        activity.DurationMinutes = activityDto.DurationMinutes;
        activity.DistanceKilometers = activityDto.DistanceKilometers;
        activity.Notes = activityDto.Notes;

        await _dbContext.SaveChangesAsync();

        return ToDto(activity);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteActivityAsync(string userId, int activityId)
    {
        var activity = await _dbContext.Activities
            .FirstOrDefaultAsync(activity => activity.UserId == userId && activity.Id == activityId);

        if (activity is null)
        {
            return false;
        }

        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps an <see cref="Models.Entities.Activity"/> entity to its DTO representation.
    /// </summary>
    /// <param name="activity">The activity entity to map.</param>
    /// <returns>The mapped DTO.</returns>
    private static ActivityDto ToDto(Models.Entities.Activity activity) => new(
        activity.Id,
        activity.Sport,
        activity.ActivityDate,
        activity.DurationMinutes,
        activity.DistanceKilometers,
        activity.Notes,
        activity.CreatedAtUtc);
}
