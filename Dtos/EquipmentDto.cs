using Triathlon.Api.Models.Enums;

namespace Triathlon.Api.Dtos;

/// <summary>
/// Represents the full round-trip shape of an equipment item, used for both
/// create/update requests and read responses.
/// </summary>
/// <param name="Id">The unique identifier of the equipment item. Ignored on create.</param>
/// <param name="Category">The category of the equipment item.</param>
/// <param name="Name">The model name of the equipment item, if provided.</param>
/// <param name="Brand">The brand of the equipment item, if provided.</param>
/// <param name="SizeOrDetails">The size or other distinguishing details, if provided.</param>
/// <param name="PurchaseDate">The purchase date of the equipment item, if provided.</param>
/// <param name="Notes">Free-form notes about the equipment item, if provided.</param>
public record EquipmentDto(
    int Id,
    EquipmentCategory Category,
    string? Name,
    string? Brand,
    string? SizeOrDetails,
    DateOnly? PurchaseDate,
    string? Notes);
