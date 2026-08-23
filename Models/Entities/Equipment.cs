using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Models.Entities;

/// <summary>
/// Represents a piece of training equipment owned by an athlete.
/// </summary>
public class Equipment
{
    /// <summary>
    /// Gets or sets the unique identifier of the equipment item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who owns this equipment item.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the navigation property to the owning user.
    /// </summary>
    public ApplicationUser? User { get; set; }

    /// <summary>
    /// Gets or sets the category of the equipment item.
    /// </summary>
    public EquipmentCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the model name of the equipment item, if provided.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the brand of the equipment item, if provided.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the size or other distinguishing details, if provided.
    /// </summary>
    public string? SizeOrDetails { get; set; }

    /// <summary>
    /// Gets or sets the purchase date of the equipment item, if provided.
    /// </summary>
    public DateOnly? PurchaseDate { get; set; }

    /// <summary>
    /// Gets or sets free-form notes about the equipment item.
    /// </summary>
    public string? Notes { get; set; }
}
