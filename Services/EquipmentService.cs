using Microsoft.EntityFrameworkCore;
using Triathlon.Api.Data;
using Triathlon.Api.Dtos;

namespace Triathlon.Api.Services;

/// <summary>
/// Provides CRUD operations over a user's equipment items, always scoped to the authenticated
/// user's identifier.
/// </summary>
public class EquipmentService : IEquipmentService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used to access equipment data.</param>
    public EquipmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EquipmentDto>> GetEquipmentItemsAsync(string userId)
    {
        var equipmentItems = await _dbContext.EquipmentItems
            .Where(equipment => equipment.UserId == userId)
            .OrderBy(equipment => equipment.Category)
            .ToListAsync();

        return equipmentItems.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<EquipmentDto?> GetEquipmentItemByIdAsync(string userId, int equipmentId)
    {
        var equipment = await _dbContext.EquipmentItems
            .FirstOrDefaultAsync(equipment => equipment.UserId == userId && equipment.Id == equipmentId);

        return equipment is null ? null : ToDto(equipment);
    }

    /// <inheritdoc />
    public async Task<EquipmentDto> CreateEquipmentItemAsync(string userId, EquipmentDto equipmentDto)
    {
        var equipment = new Models.Entities.Equipment
        {
            UserId = userId,
            Category = equipmentDto.Category,
            Name = equipmentDto.Name,
            Brand = equipmentDto.Brand,
            SizeOrDetails = equipmentDto.SizeOrDetails,
            PurchaseDate = equipmentDto.PurchaseDate,
            Notes = equipmentDto.Notes
        };

        _dbContext.EquipmentItems.Add(equipment);
        await _dbContext.SaveChangesAsync();

        return ToDto(equipment);
    }

    /// <inheritdoc />
    public async Task<EquipmentDto?> UpdateEquipmentItemAsync(string userId, int equipmentId, EquipmentDto equipmentDto)
    {
        var equipment = await _dbContext.EquipmentItems
            .FirstOrDefaultAsync(equipment => equipment.UserId == userId && equipment.Id == equipmentId);

        if (equipment is null)
        {
            return null;
        }

        equipment.Category = equipmentDto.Category;
        equipment.Name = equipmentDto.Name;
        equipment.Brand = equipmentDto.Brand;
        equipment.SizeOrDetails = equipmentDto.SizeOrDetails;
        equipment.PurchaseDate = equipmentDto.PurchaseDate;
        equipment.Notes = equipmentDto.Notes;

        await _dbContext.SaveChangesAsync();

        return ToDto(equipment);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteEquipmentItemAsync(string userId, int equipmentId)
    {
        var equipment = await _dbContext.EquipmentItems
            .FirstOrDefaultAsync(equipment => equipment.UserId == userId && equipment.Id == equipmentId);

        if (equipment is null)
        {
            return false;
        }

        _dbContext.EquipmentItems.Remove(equipment);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps an <see cref="Models.Entities.Equipment"/> entity to its DTO representation.
    /// </summary>
    /// <param name="equipment">The equipment entity to map.</param>
    /// <returns>The mapped DTO.</returns>
    private static EquipmentDto ToDto(Models.Entities.Equipment equipment) => new(
        equipment.Id,
        equipment.Category,
        equipment.Name,
        equipment.Brand,
        equipment.SizeOrDetails,
        equipment.PurchaseDate,
        equipment.Notes);
}
