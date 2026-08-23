using Microsoft.EntityFrameworkCore;
using Triathlon.Api.Data;
using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Provides CRUD operations over a user's planned training sessions, always scoped to
/// the authenticated user's identifier.
/// </summary>
public class TrainingScheduleService : ITrainingScheduleService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrainingScheduleService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to access training session data.</param>
    public TrainingScheduleService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TrainingSessionDto>> GetTrainingSessionsAsync(string userId)
    {
        var trainingSessions = await _dbContext.TrainingSessions
            .Where(trainingSession => trainingSession.UserId == userId)
            .OrderBy(trainingSession => trainingSession.ScheduledDate)
            .ToListAsync();

        return trainingSessions.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<TrainingSessionDto?> GetTrainingSessionByIdAsync(string userId, int trainingSessionId)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .FirstOrDefaultAsync(trainingSession =>
                trainingSession.UserId == userId && trainingSession.Id == trainingSessionId);

        return trainingSession is null ? null : ToDto(trainingSession);
    }

    /// <inheritdoc />
    public async Task<TrainingSessionDto> CreateTrainingSessionAsync(
        string userId, TrainingSessionDto trainingSessionDto)
    {
        var trainingSession = new Models.Entities.TrainingSession
        {
            UserId = userId,
            Sport = trainingSessionDto.Sport,
            ScheduledDate = trainingSessionDto.ScheduledDate,
            ScheduledStartTime = trainingSessionDto.ScheduledStartTime,
            DurationMinutes = trainingSessionDto.DurationMinutes,
            Notes = trainingSessionDto.Notes,
            IsCompleted = trainingSessionDto.IsCompleted
        };

        _dbContext.TrainingSessions.Add(trainingSession);
        await _dbContext.SaveChangesAsync();

        return ToDto(trainingSession);
    }

    /// <inheritdoc />
    public async Task<TrainingSessionDto?> UpdateTrainingSessionAsync(
        string userId, int trainingSessionId, TrainingSessionDto trainingSessionDto)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .FirstOrDefaultAsync(trainingSession =>
                trainingSession.UserId == userId && trainingSession.Id == trainingSessionId);

        if (trainingSession is null)
        {
            return null;
        }

        trainingSession.Sport = trainingSessionDto.Sport;
        trainingSession.ScheduledDate = trainingSessionDto.ScheduledDate;
        trainingSession.ScheduledStartTime = trainingSessionDto.ScheduledStartTime;
        trainingSession.DurationMinutes = trainingSessionDto.DurationMinutes;
        trainingSession.Notes = trainingSessionDto.Notes;
        trainingSession.IsCompleted = trainingSessionDto.IsCompleted;

        await _dbContext.SaveChangesAsync();

        return ToDto(trainingSession);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTrainingSessionAsync(string userId, int trainingSessionId)
    {
        var trainingSession = await _dbContext.TrainingSessions
            .FirstOrDefaultAsync(trainingSession =>
                trainingSession.UserId == userId && trainingSession.Id == trainingSessionId);

        if (trainingSession is null)
        {
            return false;
        }

        _dbContext.TrainingSessions.Remove(trainingSession);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps a <see cref="Models.Entities.TrainingSession"/> entity to its DTO representation.
    /// </summary>
    /// <param name="trainingSession">The training session entity to map.</param>
    /// <returns>The mapped DTO.</returns>
    private static TrainingSessionDto ToDto(Models.Entities.TrainingSession trainingSession) => new(
        trainingSession.Id,
        trainingSession.Sport,
        trainingSession.ScheduledDate,
        trainingSession.ScheduledStartTime,
        trainingSession.DurationMinutes,
        trainingSession.Notes,
        trainingSession.IsCompleted);
}
