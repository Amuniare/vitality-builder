using Microsoft.AspNetCore.Mvc;
using VitalityBuilder.Domain.Dtos.Character;
using VitalityBuilder.Domain.Errors;
using VitalityBuilder.Interfaces.Services;
using VitalityBuilder.Services.Character;

namespace VitalityBuilder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;
    private readonly IValidationService _validationService;
    private readonly ILogger<CharacterController> _logger;

    public CharacterController(
        ICharacterService characterService,
        IValidationService validationService,
        ILogger<CharacterController> logger)
    {
        _characterService = characterService;
        _validationService = validationService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new character with the specified attributes and archetypes
    /// </summary>
    /// <param name="request">Character creation details</param>
    /// <returns>The created character or validation errors</returns>
    /// <response code="201">Character created successfully</response>
    /// <response code="400">Invalid character data</response>
    [HttpPost]
    [ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterDto request)
    {
        try
        {
            _logger.LogInformation("Creating new character with name: {Name}", request.Name);

            // Validate the creation request
            var validationResult = await _validationService.ValidateCharacterCreation(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Character creation validation failed: {Errors}", 
                    string.Join(", ", validationResult.Errors));
                    
                return BadRequest(new ErrorResponse
                {
                    Message = "Character creation validation failed",
                    Details = validationResult.Errors
                });
            }

            // Process the character creation
            var result = await _characterService.CreateCharacterAsync(request);
            
            _logger.LogInformation("Successfully created character {Id}", result.Id);
            
            return CreatedAtAction(
                nameof(GetCharacter), 
                new { id = result.Id }, 
                result
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating character");
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while creating the character"
            });
        }
    }

    /// <summary>
    /// Retrieves a character by their ID
    /// </summary>
    /// <param name="id">Character ID</param>
    /// <returns>The requested character or not found</returns>
    /// <response code="200">Character found</response>
    /// <response code="404">Character not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacter(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving character {Id}", id);

            var character = await _characterService.GetCharacterAsync(id);
            if (character == null)
            {
                _logger.LogWarning("Character {Id} not found", id);
                return NotFound();
            }

            return Ok(character);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving character {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while retrieving the character"
            });
        }
    }

    /// <summary>
    /// Updates an existing character's attributes and abilities
    /// </summary>
    /// <param name="id">Character ID</param>
    /// <param name="request">Update details</param>
    /// <returns>The updated character or validation errors</returns>
    /// <response code="200">Character updated successfully</response>
    /// <response code="400">Invalid update data</response>
    /// <response code="404">Character not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCharacter(
        int id, 
        [FromBody] UpdateCharacterDto request)
    {
        try
        {
            _logger.LogInformation("Updating character {Id}", id);

            // Validate the update request
            var validationResult = await _validationService.ValidateCharacterUpdate(id, request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Character update validation failed: {Errors}",
                    string.Join(", ", validationResult.Errors));
                    
                return BadRequest(new ErrorResponse
                {
                    Message = "Character update validation failed",
                    Details = validationResult.Errors
                });
            }

            // Process the character update
            var result = await _characterService.UpdateCharacterAsync(id, request);
            if (result == null)
            {
                _logger.LogWarning("Character {Id} not found for update", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully updated character {Id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating character {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while updating the character"
            });
        }
    }

    /// <summary>
    /// Updates a character's combat attributes
    /// </summary>
    /// <param name="id">Character ID</param>
    /// <param name="request">Combat attribute updates</param>
    /// <returns>The updated character or validation errors</returns>
    [HttpPut("{id}/combat-attributes")]
    [ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCombatAttributes(
        int id, 
        [FromBody] UpdateCombatAttributesDto request)
    {
        try
        {
            _logger.LogInformation("Updating combat attributes for character {Id}", id);

            // Validate the attribute updates
            var validationResult = await _validationService
                .ValidateCombatAttributeUpdate(id, request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Combat attribute update validation failed: {Errors}",
                    string.Join(", ", validationResult.Errors));
                    
                return BadRequest(new ErrorResponse
                {
                    Message = "Combat attribute update validation failed",
                    Details = validationResult.Errors
                });
            }

            // Process the attribute update
            var result = await _characterService.UpdateCombatAttributesAsync(id, request);
            if (result == null)
            {
                _logger.LogWarning("Character {Id} not found for combat attribute update", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully updated combat attributes for character {Id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating combat attributes for character {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while updating combat attributes"
            });
        }
    }

    /// <summary>
    /// Updates a character's utility attributes
    /// </summary>
    /// <param name="id">Character ID</param>
    /// <param name="request">Utility attribute updates</param>
    /// <returns>The updated character or validation errors</returns>
    [HttpPut("{id}/utility-attributes")]
    [ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUtilityAttributes(
        int id, 
        [FromBody] UpdateUtilityAttributesDto request)
    {
        try
        {
            _logger.LogInformation("Updating utility attributes for character {Id}", id);

            // Validate the attribute updates
            var validationResult = await _validationService
                .ValidateUtilityAttributeUpdate(id, request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Utility attribute update validation failed: {Errors}",
                    string.Join(", ", validationResult.Errors));
                    
                return BadRequest(new ErrorResponse
                {
                    Message = "Utility attribute update validation failed",
                    Details = validationResult.Errors
                });
            }

            // Process the attribute update
            var result = await _characterService.UpdateUtilityAttributesAsync(id, request);
            if (result == null)
            {
                _logger.LogWarning("Character {Id} not found for utility attribute update", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully updated utility attributes for character {Id}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating utility attributes for character {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while updating utility attributes"
            });
        }
    }

    /// <summary>
    /// Deletes a character
    /// </summary>
    /// <param name="id">Character ID</param>
    /// <returns>Success or not found</returns>
    /// <response code="204">Character deleted successfully</response>
    /// <response code="404">Character not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacter(int id)
    {
        try
        {
            _logger.LogInformation("Deleting character {Id}", id);

            var result = await _characterService.DeleteCharacterAsync(id);
            if (!result)
            {
                _logger.LogWarning("Character {Id} not found for deletion", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully deleted character {Id}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting character {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Message = "An unexpected error occurred while deleting the character"
            });
        }
    }
}