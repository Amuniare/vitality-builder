using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VitalityBuilder.Api;

[ApiController]
[Route("api/[controller]")]
public class CharactersController(VitalityBuilderContext context) : ControllerBase
{
    /// <summary>
    /// Creates a new character
    /// </summary>
    /// <param name="character">The character to create</param>
    /// <returns>The created character</returns>
    /// <response code="201">Returns the newly created character</response>
    /// <response code="400">If the character data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Character>> CreateCharacter(Character character)
    {
        if (!ValidateCharacter(character))
        {
            return BadRequest("Invalid character data");
        }

        context.Characters.Add(character);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCharacter),
            new { id = character.Id },
            character);
    }

    /// <summary>
    /// Gets a specific character by id
    /// </summary>
    /// <param name="id">The character's id</param>
    /// <returns>The requested character</returns>
    /// <response code="200">Returns the requested character</response>
    /// <response code="404">If the character is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Character>> GetCharacter(int id)
    {
        var character = await context.Characters
            .Include(c => c.CombatAttributes)
            .Include(c => c.UtilityAttributes)
            .Include(c => c.Expertise)
            .Include(c => c.SpecialAttacks)
            .Include(c => c.UniquePowers)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null)
        {
            return NotFound();
        }

        return character;
    }

    /// <summary>
    /// Gets all characters
    /// </summary>
    /// <returns>List of all characters</returns>
    /// <response code="200">Returns all characters</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
    {
        return await context.Characters
            .Include(c => c.CombatAttributes)
            .Include(c => c.UtilityAttributes)
            .ToListAsync();
    }

    /// <summary>
    /// Updates a specific character
    /// </summary>
    /// <param name="id">The character's id</param>
    /// <param name="character">The updated character data</param>
    /// <returns>No content</returns>
    /// <response code="204">If the character was successfully updated</response>
    /// <response code="400">If the character data is invalid</response>
    /// <response code="404">If the character is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCharacter(int id, Character character)
    {
        if (id != character.Id)
        {
            return BadRequest();
        }

        if (!ValidateCharacter(character))
        {
            return BadRequest("Invalid character data");
        }

        context.Entry(character).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CharacterExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific character
    /// </summary>
    /// <param name="id">The character's id</param>
    /// <returns>No content</returns>
    /// <response code="204">If the character was successfully deleted</response>
    /// <response code="404">If the character is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCharacter(int id)
    {
        var character = await context.Characters.FindAsync(id);
        if (character == null)
        {
            return NotFound();
        }

        context.Characters.Remove(character);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool CharacterExists(int id)
    {
        return context.Characters.Any(e => e.Id == id);
    }

    private static bool ValidateCharacter(Character character)
    {
        // Basic validation
        if (character.Tier < 1 || character.Tier > 10)
        {
            return false;
        }

        // Validate main pool points (10 x Tier)
        if (character.MainPoolPoints > character.Tier * 10)
        {
            return false;
        }

        // Validate combat attributes don't exceed Tier
        var combatAttrs = character.CombatAttributes;
        if (combatAttrs.Focus > character.Tier ||
            combatAttrs.Power > character.Tier ||
            combatAttrs.Mobility > character.Tier ||
            combatAttrs.Endurance > character.Tier)
        {
            return false;
        }

        // Validate utility attributes don't exceed Tier
        var utilityAttrs = character.UtilityAttributes;
        if (utilityAttrs.Awareness > character.Tier ||
            utilityAttrs.Communication > character.Tier ||
            utilityAttrs.Intelligence > character.Tier)
        {
            return false;
        }

        // Validate number of special attacks equals Tier
        if (character.SpecialAttacks.Count > character.Tier)
        {
            return false;
        }

        return true;
    }
}