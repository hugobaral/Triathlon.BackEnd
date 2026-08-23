using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triathlon.Api.Dtos;
using Triathlon.Api.Services;

namespace Triathlon.Api.Controllers;

/// <summary>
/// Manages the authenticated user's equipment items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentController"/> class.
    /// </summary>
    /// <param name="equipmentService">The service used to manage equipment items.</param>
    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    /// <summary>
    /// Retrieves the current user's equipment items.
    /// </summary>
    /// <returns>The user's equipment items.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EquipmentDto>>> GetEquipmentItems()
    {
        var equipmentItems = await _equipmentService.GetEquipmentItemsAsync(GetCurrentUserId());
        return Ok(equipmentItems);
    }

    /// <summary>
    /// Retrieves a single equipment item belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the equipment item to retrieve.</param>
    /// <returns>The matching equipment item, or 404 Not Found.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EquipmentDto>> GetEquipmentItemById(int id)
    {
        var equipmentItem = await _equipmentService.GetEquipmentItemByIdAsync(GetCurrentUserId(), id);
        return equipmentItem is null ? NotFound() : Ok(equipmentItem);
    }

    /// <summary>
    /// Creates a new equipment item for the current user.
    /// </summary>
    /// <param name="equipmentDto">The equipment data to create.</param>
    /// <returns>The created equipment item.</returns>
    [HttpPost]
    public async Task<ActionResult<EquipmentDto>> CreateEquipmentItem(EquipmentDto equipmentDto)
    {
        var createdEquipmentItem = await _equipmentService.CreateEquipmentItemAsync(GetCurrentUserId(), equipmentDto);
        return CreatedAtAction(
            nameof(GetEquipmentItemById), new { id = createdEquipmentItem.Id }, createdEquipmentItem);
    }

    /// <summary>
    /// Updates an existing equipment item belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the equipment item to update.</param>
    /// <param name="equipmentDto">The updated equipment data.</param>
    /// <returns>The updated equipment item, or 404 Not Found.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EquipmentDto>> UpdateEquipmentItem(int id, EquipmentDto equipmentDto)
    {
        var updatedEquipmentItem = await _equipmentService.UpdateEquipmentItemAsync(GetCurrentUserId(), id, equipmentDto);
        return updatedEquipmentItem is null ? NotFound() : Ok(updatedEquipmentItem);
    }

    /// <summary>
    /// Deletes an existing equipment item belonging to the current user.
    /// </summary>
    /// <param name="id">The identifier of the equipment item to delete.</param>
    /// <returns>204 No Content on success, or 404 Not Found.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEquipmentItem(int id)
    {
        var wasDeleted = await _equipmentService.DeleteEquipmentItemAsync(GetCurrentUserId(), id);
        return wasDeleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves the identifier of the currently authenticated user from the JWT claims.
    /// </summary>
    /// <returns>The current user's identifier.</returns>
    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("The current user's identifier claim is missing.");
}
