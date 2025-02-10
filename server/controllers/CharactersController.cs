using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using VitalityBuilder.Api.Models.DTOs;
using VitalityBuilder.Api.Models.Entities;
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
    private readonly IValidator<CreateCharacterDto> _createCharacterValidator;
    private readonly IValidator<CharacterArchetypesDto> _archetypesValidator;

    public CharactersController(
        VitalityBuilderContext context,
        ILogger<CharactersController> logger,
        ICharacterArchetypesService archetypeService,
        IValidator<CreateCharacterDto> createCharacterValidator,
        IValidator<CharacterArchetypesDto> archetypesValidator)
    {
        _context = context;
        _logger = logger;
        _archetypeService = archetypeService;
        _createCharacterValidator = createCharacterValidator;
        _archetypesValidator = archetypesValidator;
    }

    /// <summary>
    /// Creates a new character with basic attributes
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CharacterEntity>> CreateCharacter([FromBody] CreateCharacterDto dto)
    {
        _logger.LogInformation("Received character creation request for {CharacterName}", dto.Name);

        // Validate the DTO
        var validationResult = await _createCharacterValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try 
        {
            // Validate attribute points against tier limits
            if (!_validationService.IsValid(dto))
            {
                return BadRequest("Attribute points exceed tier limits");
            }

            var character = new Character
            {
                Name = dto.Name,
                Tier = dto.Tier,
                CombatAttributes = new CombatAttributes
                {
                    Focus = dto.CombatAttributes.Focus,
                    Power = dto.CombatAttributes.Power,
                    Mobility = dto.CombatAttributes.Mobility,
                    Endurance = dto.CombatAttributes.Endurance,
                    Total = dto.CombatAttributes.Total
                },
                UtilityAttributes = new UtilityAttributes
                {
                    Awareness = dto.UtilityAttributes.Awareness,
                    Communication = dto.UtilityAttributes.Communication,
                    Intelligence = dto.UtilityAttributes.Intelligence
                }
            };

            _context.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacter), new { id = character.Id }, character);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating character {CharacterName}", dto.Name);
            return StatusCode(500, "An error occurred while creating the character");
        }
    }

    /// <summary>
    /// Updates the archetypes for an existing character
    /// </summary>
    [HttpPut("{id}/archetypes")]
    public async Task<IActionResult> UpdateArchetypes(int id, [FromBody] CharacterArchetypesDto dto)
    {
        var character = await _context.Characters
            .Include(c => c.CharacterArchetypes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null) return NotFound();

        // Validate the archetypes DTO
        var validationResult = await _archetypesValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

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

    /// <summary>
    /// Gets a character by ID including all related data
    /// </summary>
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

    private readonly ValidateAttributePointsService _validationService;
    public CharactersController(
        VitalityBuilderContext context,
        ILogger<CharactersController> logger,
        ICharacterArchetypesService archetypeService,
        IValidator<CreateCharacterDto> createCharacterValidator,
        IValidator<CharacterArchetypesDto> archetypesValidator,
        ValidateAttributePointsService validationService)
    {
        _context = context;
        _logger = logger;
        _archetypeService = archetypeService;
        _createCharacterValidator = createCharacterValidator;
        _archetypesValidator = archetypesValidator;
        _validationService = validationService;
    }

}