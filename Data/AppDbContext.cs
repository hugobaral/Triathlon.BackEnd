using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Triathlon.Api.Models.Entities;

namespace Triathlon.Api.Data;

/// <summary>
/// The Entity Framework Core database context for the Triathlon training tracker,
/// combining ASP.NET Core Identity tables with the application's own domain tables.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options used to configure this context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the completed training activities logged by users.
    /// </summary>
    public DbSet<Activity> Activities => Set<Activity>();

    /// <summary>
    /// Gets or sets the planned training sessions on users' calendars.
    /// </summary>
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();

    /// <summary>
    /// Gets or sets the equipment items owned by users.
    /// </summary>
    public DbSet<Equipment> EquipmentItems => Set<Equipment>();

    /// <summary>
    /// Gets or sets the hashed refresh tokens issued to users.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// Configures the entity model, including required foreign keys and cascade delete
    /// behavior linking each domain table back to its owning <see cref="ApplicationUser"/>.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Activity>(entityBuilder =>
        {
            entityBuilder.Property(activity => activity.UserId).IsRequired();
            entityBuilder.HasOne(activity => activity.User)
                .WithMany()
                .HasForeignKey(activity => activity.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrainingSession>(entityBuilder =>
        {
            entityBuilder.Property(trainingSession => trainingSession.UserId).IsRequired();
            entityBuilder.HasOne(trainingSession => trainingSession.User)
                .WithMany()
                .HasForeignKey(trainingSession => trainingSession.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Equipment>(entityBuilder =>
        {
            entityBuilder.Property(equipment => equipment.UserId).IsRequired();
            entityBuilder.HasOne(equipment => equipment.User)
                .WithMany()
                .HasForeignKey(equipment => equipment.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(entityBuilder =>
        {
            entityBuilder.Property(refreshToken => refreshToken.UserId).IsRequired();
            entityBuilder.HasOne(refreshToken => refreshToken.User)
                .WithMany()
                .HasForeignKey(refreshToken => refreshToken.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
