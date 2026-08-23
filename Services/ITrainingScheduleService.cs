using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Defines CRUD operations over a user's planned training sessions.
/// </summary>
public interface ITrainingScheduleService
{
    /// <summary>
    /// Retrieves the training sessions belonging to a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <returns>A task that resolves to the user's training sessions, ordered by scheduled date.</returns>
    Task<IReadOnlyList<TrainingSessionDto>> GetTrainingSessionsAsync(string userId);

    /// <summary>
    /// Retrieves a single training session belonging to a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="trainingSessionId">The identifier of the training session to retrieve.</param>
    /// <returns>A task that resolves to the matching training session, or <c>null</c> if not found.</returns>
    Task<TrainingSessionDto?> GetTrainingSessionByIdAsync(string userId, int trainingSessionId);

    /// <summary>
    /// Creates a new training session for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="trainingSessionDto">The training session data to create.</param>
    /// <returns>A task that resolves to the created training session.</returns>
    Task<TrainingSessionDto> CreateTrainingSessionAsync(string userId, TrainingSessionDto trainingSessionDto);

    /// <summary>
    /// Updates an existing training session for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="trainingSessionId">The identifier of the training session to update.</param>
    /// <param name="trainingSessionDto">The updated training session data.</param>
    /// <returns>A task that resolves to the updated training session, or <c>null</c> if not found.</returns>
    Task<TrainingSessionDto?> UpdateTrainingSessionAsync(
        string userId, int trainingSessionId, TrainingSessionDto trainingSessionDto);

    /// <summary>
    /// Deletes an existing training session for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="trainingSessionId">The identifier of the training session to delete.</param>
    /// <returns>A task that resolves to <c>true</c> if the training session was deleted, or <c>false</c> if not found.</returns>
    Task<bool> DeleteTrainingSessionAsync(string userId, int trainingSessionId);
}
