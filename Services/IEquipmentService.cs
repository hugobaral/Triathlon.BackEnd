using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Defines CRUD operations over a user's equipment items.
/// </summary>
public interface IEquipmentService
{
    /// <summary>
    /// Retrieves the equipment items belonging to a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <returns>A task that resolves to the user's equipment items.</returns>
    Task<IReadOnlyList<EquipmentDto>> GetEquipmentItemsAsync(string userId);

    /// <summary>
    /// Retrieves a single equipment item belonging to a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="equipmentId">The identifier of the equipment item to retrieve.</param>
    /// <returns>A task that resolves to the matching equipment item, or <c>null</c> if not found.</returns>
    Task<EquipmentDto?> GetEquipmentItemByIdAsync(string userId, int equipmentId);

    /// <summary>
    /// Creates a new equipment item for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="equipmentDto">The equipment data to create.</param>
    /// <returns>A task that resolves to the created equipment item.</returns>
    Task<EquipmentDto> CreateEquipmentItemAsync(string userId, EquipmentDto equipmentDto);

    /// <summary>
    /// Updates an existing equipment item for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="equipmentId">The identifier of the equipment item to update.</param>
    /// <param name="equipmentDto">The updated equipment data.</param>
    /// <returns>A task that resolves to the updated equipment item, or <c>null</c> if not found.</returns>
    Task<EquipmentDto?> UpdateEquipmentItemAsync(string userId, int equipmentId, EquipmentDto equipmentDto);

    /// <summary>
    /// Deletes an existing equipment item for a user.
    /// </summary>
    /// <param name="userId">The identifier of the owning user.</param>
    /// <param name="equipmentId">The identifier of the equipment item to delete.</param>
    /// <returns>A task that resolves to <c>true</c> if the equipment item was deleted, or <c>false</c> if not found.</returns>
    Task<bool> DeleteEquipmentItemAsync(string userId, int equipmentId);
}
