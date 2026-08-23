using Triathlon.Api.Dtos;
using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Services;

/// <summary>
/// Defines CRUD operations over a user's completed training activities.
/// </summary>
public interface IActivityService
{
    /// <summary>
    /// Retrieves the activities belonging to a user, optionally filtered by sport and date range.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="sport">An optional sport to filter by.</param>
    /// <param name="startDate">An optional inclusive lower bound on the activity date.</param>
    /// <param name="endDate">An optional inclusive upper bound on the activity date.</param>
    /// <returns>A task that resolves to the matching activities, ordered by date descending.</returns>
    Task<IReadOnlyList<ActivityDto>> GetActivitiesAsync(
        string userId, SportType? sport, DateOnly? startDate, DateOnly? endDate);

    /// <summary>
    /// Retrieves a single activity belonging to a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="activityId">The identifier of the activity to retrieve.</param>
    /// <returns>A task that resolves to the matching activity, or <c>null</c> if not found.</returns>
    Task<ActivityDto?> GetActivityByIdAsync(string userId, int activityId);

    /// <summary>
    /// Creates a new activity for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="activityDto">The activity data to create.</param>
    /// <returns>A task that resolves to the created activity.</returns>
    Task<ActivityDto> CreateActivityAsync(string userId, ActivityDto activityDto);

    /// <summary>
    /// Updates an existing activity for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="activityId">The identifier of the activity to update.</param>
    /// <param name="activityDto">The updated activity data.</param>
    /// <returns>A task that resolves to the updated activity, or <c>null</c> if not found.</returns>
    Task<ActivityDto?> UpdateActivityAsync(string userId, int activityId, ActivityDto activityDto);

    /// <summary>
    /// Deletes an existing activity for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="activityId">The identifier of the activity to delete.</param>
    /// <returns>A task that resolves to <c>true</c> if the activity was deleted, or <c>false</c> if not found.</returns>
    Task<bool> DeleteActivityAsync(string userId, int activityId);
}
