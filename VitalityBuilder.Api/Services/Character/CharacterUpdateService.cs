using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.Dtos.Character;
using VitalityBuilder.Api.Domain.Errors;
using VitalityBuilder.Api.Infrastructure.Security;
using VitalityBuilder.Api.Interfaces.Services;

namespace VitalityBuilder.Api.Services.Character;

public class CharacterUpdateService : ICharacterUpdateService
{
    private readonly ICharacterRepository _repository;
    private readonly ICharacterStatCalculator _statCalculator;
    private readonly IPointPoolCalculator _pointCalculator;
    private readonly IValidationService _validationService;
    private readonly ILogger<CharacterUpdateService> _logger;

    public CharacterUpdateService(
        ICharacterRepository repository,
        ICharacterStatCalculator statCalculator,
        IPointPoolCalculator pointCalculator,
        IValidationService validationService,
        ILogger<CharacterUpdateService> logger)
    {
        _repository = repository;
        _statCalculator = statCalculator;
        _pointCalculator = pointCalculator;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<CharacterResponseDto> UpdateCharacterAsync(int id, UpdateCharacterDto request)
    {
        try
        {
            // Get existing character
            var character = await _repository.GetCharacterWithDetailsAsync(id);
            if (character == null)
            {
                throw new NotFoundException($"Character with ID {id} not found");
            }

            // Create snapshot for change tracking
            var snapshot = CreateCharacterSnapshot(character);

            // Validate the update request
            var validationResult = await _validationService.ValidateCharacterUpdate(character, request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Character update validation failed: {Errors}", 
                    string.Join(", ", validationResult.Errors));
                throw new ValidationException(validationResult.Errors);
            }

            // Update basic information
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                character.Name = InputSanitizer.SanitizeName(request.Name);
            }

            // Update tier if changed
            if (request.Tier.HasValue && request.Tier.Value != character.Tier)
            {
                await UpdateCharacterTier(character, request.Tier.Value);
            }

            // Update attributes if provided
            if (request.CombatAttributes != null)
            {
                await UpdateCombatAttributes(character, request.CombatAttributes);
            }

            if (request.UtilityAttributes != null)
            {
                await UpdateUtilityAttributes(character, request.UtilityAttributes);
            }

            // Update archetypes if provided
            if (request.Archetypes != null)
            {
                await UpdateArchetypes(character, request.Archetypes);
            }

            // Recalculate stats
            var stats = _statCalculator.CalculateAllStats(character);
            var points = _pointCalculator.CalculateAllPools(character);

            // Validate final state
            var finalValidation = await ValidateFinalState(character, snapshot);
            if (!finalValidation.IsValid)
            {
                throw new ValidationException(finalValidation.Errors);
            }

            // Save changes
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Updated character {Id}", id);

            // Create response
            return await CreateResponseDto(character, stats, points, finalValidation.Warnings);
        }
        catch (Exception ex) when (ex is not ValidationException && ex is not NotFoundException)
        {
            _logger.LogError(ex, "Error updating character {Id}", id);
            throw;
        }
    }

    private CharacterSnapshot CreateCharacterSnapshot(Domain.Character.Character character)
    {
        return new CharacterSnapshot
        {
            Tier = character.Tier,
            CombatAttributes = character.CombatAttributes.Clone(),
            UtilityAttributes = character.UtilityAttributes.Clone(),
            Archetypes = character.Archetypes.Clone()
        };
    }

    private async Task UpdateCharacterTier(Domain.Character.Character character, int newTier)
    {
        // Validate tier change
        var tierValidation = await _validationService.ValidateTierChange(character, newTier);
        if (!tierValidation.IsValid)
        {
            throw new ValidationException(tierValidation.Errors);
        }

        character.Tier = newTier;
    }

    private async Task UpdateCombatAttributes(Domain.Character.Character character, UpdateCombatAttributesDto attributes)
    {
        // Validate attribute changes
        var attributeValidation = await _validationService.ValidateCombatAttributeUpdate(character, attributes);
        if (!attributeValidation.IsValid)
        {
            throw new ValidationException(attributeValidation.Errors);
        }

        character.CombatAttributes.Focus = attributes.Focus;
        character.CombatAttributes.Power = attributes.Power;
        character.CombatAttributes.Mobility = attributes.Mobility;
        character.CombatAttributes.Endurance = attributes.Endurance;
    }

    private async Task UpdateUtilityAttributes(Domain.Character.Character character, UpdateUtilityAttributesDto attributes)
    {
        // Validate attribute changes
        var attributeValidation = await _validationService.ValidateUtilityAttributeUpdate(character, attributes);
        if (!attributeValidation.IsValid)
        {
            throw new ValidationException(attributeValidation.Errors);
        }

        character.UtilityAttributes.Awareness = attributes.Awareness;
        character.UtilityAttributes.Communication = attributes.Communication;
        character.UtilityAttributes.Intelligence = attributes.Intelligence;
    }

    private async Task UpdateArchetypes(Domain.Character.Character character, UpdateArchetypesDto archetypes)
    {
        // Validate archetype changes
        var archetypeValidation = await _validationService.ValidateArchetypeUpdate(character, archetypes);
        if (!archetypeValidation.IsValid)
        {
            throw new ValidationException(archetypeValidation.Errors);
        }

        character.Archetypes.MovementType = Enum.Parse<MovementArchetype>(archetypes.MovementType);
        character.Archetypes.AttackType = Enum.Parse<AttackArchetype>(archetypes.AttackType);
        character.Archetypes.EffectType = Enum.Parse<EffectArchetype>(archetypes.EffectType);
        character.Archetypes.UniqueAbility = Enum.Parse<UniqueAbilityArchetype>(archetypes.UniqueAbility);
        character.Archetypes.SpecialAttack = Enum.Parse<SpecialAttackArchetype>(archetypes.SpecialAttack);
        character.Archetypes.UtilityType = Enum.Parse<UtilityArchetype>(archetypes.UtilityType);
    }

    private async Task<ValidationResult> ValidateFinalState(
        Domain.Character.Character character, 
        CharacterSnapshot snapshot)
    {
        var result = new ValidationResult();

        // Validate points
        var pointValidation = _pointCalculator.ValidatePointAllocation(character);
        result.AddErrors(pointValidation.Errors);
        result.AddWarnings(pointValidation.Warnings);

        // Validate ability requirements
        var abilityValidation = await _validationService.ValidateAbilityRequirements(character);
        result.AddErrors(abilityValidation.Errors);
        result.AddWarnings(abilityValidation.Warnings);

        // Add warnings for significant changes
        if (character.Tier != snapshot.Tier)
        {
            result.AddWarning($"Character tier changed from {snapshot.Tier} to {character.Tier}");
        }

        return result;
    }

    private async Task<CharacterResponseDto> CreateResponseDto(
        Domain.Character.Character character,
        CombatStats stats,
        PointPools points,
        IEnumerable<string> warnings)
    {
        return new CharacterResponseDto
        {
            Id = character.Id,
            Name = character.Name,
            Tier = character.Tier,
            CombatAttributes = new()
            {
                Focus = character.CombatAttributes.Focus,
                Power = character.CombatAttributes.Power,
                Mobility = character.CombatAttributes.Mobility,
                Endurance = character.CombatAttributes.Endurance,
                Values = new()
                {
                    Avoidance = stats.Avoidance,
                    Durability = stats.Durability,
                    ResolveResistance = stats.ResolveResistance,
                    StabilityResistance = stats.StabilityResistance,
                    VitalityResistance = stats.VitalityResistance
                }
            },
            UtilityAttributes = new()
            {
                Awareness = character.UtilityAttributes.Awareness,
                Communication = character.UtilityAttributes.Communication,
                Intelligence = character.UtilityAttributes.Intelligence
            },
            Archetypes = new()
            {
                MovementType = character.Archetypes.MovementType.ToString(),
                AttackType = character.Archetypes.AttackType.ToString(),
                EffectType = character.Archetypes.EffectType.ToString(),
                UniqueAbility = character.Archetypes.UniqueAbility.ToString(),
                SpecialAttack = character.Archetypes.SpecialAttack.ToString(),
                UtilityType = character.Archetypes.UtilityType.ToString()
            },
            PointPools = new()
            {
                MainPool = points.MainPool,
                UtilityPoints = points.UtilityPoints,
                CombatAttributePoints = points.CombatAttributePoints,
                UtilityAttributePoints = points.UtilityAttributePoints,
                SpecialAttackPoints = points.SpecialAttackPoints
            },
            Warnings = warnings.ToList()
        };
    }
}

public class CharacterSnapshot
{
    public int Tier { get; set; }
    public CombatAttributes CombatAttributes { get; set; } = null!;
    public UtilityAttributes UtilityAttributes { get; set; } = null!;
    public CharacterArchetypes Archetypes { get; set; } = null!;
}