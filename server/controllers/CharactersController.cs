using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Models.DTOs;
using VitalityBuilder.Api.Models.Entities;  // Make sure this is the correct namespace
using VitalityBuilder.Api.Services;
using VitalityBuilder.Api.Infrastructure;

namespace VitalityBuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly VitalityBuilderContext _context;
    private readonly ILogger<CharactersController> _logger;
    private readonly ICharacterArchetypesService _archetypeService;

    public CharactersController(
        VitalityBuilderContext context,
        ILogger<CharactersController> logger,
        ICharacterArchetypesService archetypeService)
    {
        _context = context;
        _logger = logger;
        _archetypeService = archetypeService;
    }

    [HttpPost]
    public async Task<ActionResult<Character>> CreateCharacter(CreateCharacterDto dto)
    {
        _logger.LogInformation("Received character creation request for {CharacterName}", dto.Name);

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Character name is required");
        }

        try 
        {
            var character = new Character
            {
                Name = dto.Name,
                Tier = dto.Tier,
                CombatAttributes = new CombatAttributes
                {
                    Focus = dto.CombatAttributes.Focus,
                    Power = dto.CombatAttributes.Power,
                    Mobility = dto.CombatAttributes.Mobility,
                    Endurance = dto.CombatAttributes.Endurance
                },
                UtilityAttributes = new UtilityAttributes
                {
                    Awareness = dto.UtilityAttributes.Awareness,
                    Communication = dto.UtilityAttributes.Communication,
                    Intelligence = dto.UtilityAttributes.Intelligence
                }
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacter), new { id = character.Id }, character);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating character {CharacterName}", dto.Name);
            return StatusCode(500, "An error occurred while creating the character");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Character>> GetCharacter(int id)
    {
        var character = await _context.Characters
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

    [HttpPut("{id}/archetypes")]
    public async Task<IActionResult> UpdateArchetypes(int id, [FromBody] CharacterArchetypesDto dto)
    {
        var character = await _context.Characters
            .Include(c => c.CharacterArchetypes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null) return NotFound();

        try
        {
            var archetypes = await _archetypeService.CreateArchetypesAsync(dto, character.Id);
            
            character.SpecialAttacksPointPool = _archetypeService.CalculateSpecialAttackPoints(
                archetypes.SpecialAttackArchetype, 
                character.Tier
            );
            
            character.UtilityPointPool = _archetypeService.CalculateUtilityPoints(
                archetypes.UtilityArchetype,
                character.Tier
            );

            await _context.SaveChangesAsync();
            return Ok(character);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating archetypes for character {CharacterId}", id);
            return StatusCode(500, "Error updating archetypes");
        }
    }

    [HttpPut("{id}/combat-attributes")]
    public async Task<IActionResult> UpdateCombatAttributes(int id, [FromBody] CombatAttributesDto dto)
    {
        var character = await _context.Characters
            .Include(c => c.CombatAttributes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null) return NotFound();

        // Validate against tier limits
        if (!ValidateCombatAttributes(dto, character.Tier))
        {
            return BadRequest("Attributes exceed tier limits");
        }

        if (character.CombatAttributes != null)
        {
            character.CombatAttributes.Focus = dto.Focus;
            character.CombatAttributes.Power = dto.Power;
            character.CombatAttributes.Mobility = dto.Mobility;
            character.CombatAttributes.Endurance = dto.Endurance;
            character.CombatAttributes.Total = dto.Total;
        }

        await _context.SaveChangesAsync();
        return Ok(character);
    }

    private static bool ValidateCombatAttributes(CombatAttributesDto attributes, int tier)
    {
        return attributes.Total <= tier * 2 
            && attributes.Focus <= tier
            && attributes.Power <= tier
            && attributes.Mobility <= tier
            && attributes.Endurance <= tier;
    }
}