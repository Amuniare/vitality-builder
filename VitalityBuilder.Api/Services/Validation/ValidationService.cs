using FluentValidation;
using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.Constants;
using VitalityBuilder.Api.Domain.Dtos.Character;
using VitalityBuilder.Api.Domain.Errors;
using VitalityBuilder.Api.Infrastructure.Validation;

namespace VitalityBuilder.Api.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly ICharacterRepository _repository;
    private readonly IPointPoolCalculator _pointCalculator;
    private readonly ILogger<ValidationService> _logger;

    public ValidationService(
        ICharacterRepository repository,
        IPointPoolCalculator pointCalculator,
        ILogger<ValidationService> logger)
    {
        _repository = repository;
        _pointCalculator = pointCalculator;
        _logger = logger;
    }

    public async Task<ValidationResult> ValidateCharacterCreation(CreateCharacterDto request)
    {
        var result = new ValidationResult();

        // Basic validation using FluentValidation
        var validator = new CharacterValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            result.AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
            return result;
        }

        // Validate name uniqueness
        if (await _repository.NameExistsAsync(request.Name))
        {
            result.AddError($"Character name '{request.Name}' is already taken");
            return result;
        }

        // Validate archetype combinations
        var archetypeValidation = ValidateArchetypeCombinations(
            request.AttackArchetype,
            request.EffectArchetype,
            request.MovementArchetype,
            request.SpecialAttackArchetype);

        if (!archetypeValidation.IsValid)
        {
            result.AddErrors(archetypeValidation.Errors);
            return result;
        }

        // Add optimization warnings
        AddOptimizationWarnings(request, result);

        return result;
    }

    public async Task<ValidationResult> ValidateCharacterUpdate(
        Domain.Character.Character character,
        UpdateCharacterDto request)
    {
        var result = new ValidationResult();

        // Validate tier changes
        if (request.Tier.HasValue)
        {
            var tierValidation = ValidateTierChange(character.Tier, request.Tier.Value);
            if (!tierValidation.IsValid)
            {
                result.AddErrors(tierValidation.Errors);
                return result;
            }
        }

        // Validate name changes
        if (!string.IsNullOrWhiteSpace(request.Name) && 
            request.Name != character.Name &&
            await _repository.NameExistsAsync(request.Name))
        {
            result.AddError($"Character name '{request.Name}' is already taken");
            return result;
        }

        // Validate attribute changes
        if (request.CombatAttributes != null)
        {
            var combatValidation = ValidateCombatAttributes(
                request.CombatAttributes,
                request.Tier ?? character.Tier);

            if (!combatValidation.IsValid)
            {
                result.AddErrors(combatValidation.Errors);
                return result;
            }
        }

        if (request.UtilityAttributes != null)
        {
            var utilityValidation = ValidateUtilityAttributes(
                request.UtilityAttributes,
                request.Tier ?? character.Tier);

            if (!utilityValidation.IsValid)
            {
                result.AddErrors(utilityValidation.Errors);
                return result;
            }
        }

        // Add optimization warnings
        if (request.CombatAttributes != null || request.UtilityAttributes != null)
        {
            AddOptimizationWarnings(request, result);
        }

        return result;
    }

    public async Task<ValidationResult> ValidateAbilityRequirements(Domain.Character.Character character)
    {
        var result = new ValidationResult();

        // Validate special attack requirements
        foreach (var attack in character.SpecialAttacks)
        {
            var attackValidation = ValidateSpecialAttackRequirements(attack, character);
            if (!attackValidation.IsValid)
            {
                result.AddErrors(attackValidation.Errors);
            }
        }

        // Validate feature requirements
        foreach (var feature in character.Features)
        {
            var featureValidation = ValidateFeatureRequirements(feature, character);
            if (!featureValidation.IsValid)
            {
                result.AddErrors(featureValidation.Errors);
            }
        }

        // Validate expertise requirements
        foreach (var expertise in character.Expertise)
        {
            var expertiseValidation = ValidateExpertiseRequirements(expertise, character);
            if (!expertiseValidation.IsValid)
            {
                result.AddErrors(expertiseValidation.Errors);
            }
        }

        return result;
    }

    private ValidationResult ValidateArchetypeCombinations(
        string attackType,
        string effectType,
        string movementType,
        string specialAttackType)
    {
        var result = new ValidationResult();

        // Validate incompatible combinations
        if (effectType == "DamageSpecialist" && attackType == "DirectSpecialist")
        {
            result.AddError("Direct Specialist cannot be combined with Damage Specialist");
        }

        // Add warnings for potentially inefficient combinations
        if (attackType == "AOESpecialist" && movementType == "Swift")
        {
            result.AddWarning("AOE Specialist may have limited synergy with Swift movement");
        }

        return result;
    }

    private ValidationResult ValidateTierChange(int currentTier, int newTier)
    {
        var result = new ValidationResult();

        if (newTier < GameRuleConstants.MinimumTier || 
            newTier > GameRuleConstants.MaximumTier)
        {
            result.AddError($"Tier must be between {GameRuleConstants.MinimumTier} and {GameRuleConstants.MaximumTier}");
            return result;
        }

        if (newTier < currentTier)
        {
            result.AddError("Cannot decrease character tier");
            return result;
        }

        if (newTier - currentTier > 1)
        {
            result.AddWarning("Increasing tier by more than one level at once may require additional validation");
        }

        return result;
    }

    private ValidationResult ValidateCombatAttributes(
        UpdateCombatAttributesDto attributes,
        int tier)
    {
        var result = new ValidationResult();

        // Validate individual attributes
        if (attributes.Focus > tier)
            result.AddError($"Focus cannot exceed tier ({tier})");

        if (attributes.Power > tier)
            result.AddError($"Power cannot exceed tier ({tier})");

        if (attributes.Mobility > tier)
            result.AddError($"Mobility cannot exceed tier ({tier})");

        if (attributes.Endurance > tier)
            result.AddError($"Endurance cannot exceed tier ({tier})");

        // Validate total points
        var total = attributes.Focus + attributes.Power + 
                   attributes.Mobility + attributes.Endurance;

        if (total > tier * 2)
            result.AddError($"Total combat attributes cannot exceed {tier * 2} points");

        return result;
    }

    private ValidationResult ValidateUtilityAttributes(
        UpdateUtilityAttributesDto attributes,
        int tier)
    {
        var result = new ValidationResult();

        // Validate individual attributes
        if (attributes.Awareness > tier)
            result.AddError($"Awareness cannot exceed tier ({tier})");

        if (attributes.Communication > tier)
            result.AddError($"Communication cannot exceed tier ({tier})");

        if (attributes.Intelligence > tier)
            result.AddError($"Intelligence cannot exceed tier ({tier})");

        // Validate total points
        var total = attributes.Awareness + attributes.Communication + 
                   attributes.Intelligence;

        if (total > tier)
            result.AddError($"Total utility attributes cannot exceed {tier} points");

        return result;
    }

    private ValidationResult ValidateSpecialAttackRequirements(
        SpecialAttack attack,
        Domain.Character.Character character)
    {
        var result = new ValidationResult();

        // Validate attribute requirements
        switch (attack.AttackType)
        {
            case AttackType.Melee when character.CombatAttributes.Power < 2:
                result.AddError($"Melee attack '{attack.Name}' requires Power 2 or higher");
                break;

            case AttackType.Ranged when character.CombatAttributes.Focus < 2:
                result.AddError($"Ranged attack '{attack.Name}' requires Focus 2 or higher");
                break;
        }

        // Validate point costs
        if (attack.PointCost > character.SpecialAttackPoints)
        {
            result.AddError($"Special attack '{attack.Name}' exceeds available points");
        }

        return result;
    }

    private ValidationResult ValidateFeatureRequirements(
        CharacterFeature feature,
        Domain.Character.Character character)
    {
        var result = new ValidationResult();

        // Validate attribute requirements
        if (feature.MinimumAttribute > 0)
        {
            var attributeValue = GetAttributeValue(character, feature.RequiredAttribute);
            if (attributeValue < feature.MinimumAttribute)
            {
                result.AddError($"Feature '{feature.Name}' requires {feature.RequiredAttribute} {feature.MinimumAttribute} or higher");
            }
        }

        return result;
    }

    private ValidationResult ValidateExpertiseRequirements(
        CharacterExpertise expertise,
        Domain.Character.Character character)
    {
        var result = new ValidationResult();

        // Validate attribute requirements
        var attributeValue = GetAttributeValue(character, expertise.RequiredAttribute);
        if (attributeValue < expertise.MinimumAttribute)
        {
            result.AddError($"Expertise '{expertise.Name}' requires {expertise.RequiredAttribute} {expertise.MinimumAttribute} or higher");
        }

        return result;
    }

    private int GetAttributeValue(Domain.Character.Character character, string attributeName)
    {
        return attributeName.ToLower() switch
        {
            "focus" => character.CombatAttributes.Focus,
            "power" => character.CombatAttributes.Power,
            "mobility" => character.CombatAttributes.Mobility,
            "endurance" => character.CombatAttributes.Endurance,
            "awareness" => character.UtilityAttributes.Awareness,
            "communication" => character.UtilityAttributes.Communication,
            "intelligence" => character.UtilityAttributes.Intelligence,
            _ => throw new ArgumentException($"Unknown attribute: {attributeName}")
        };
    }

    private void AddOptimizationWarnings(dynamic request, ValidationResult result)
    {
        // Check for potentially inefficient combat builds
        if (request.Focus > 0 && request.Power > 0 && request.Mobility > 0)
        {
            result.AddWarning("Spreading points across Focus, Power, and Mobility may reduce combat effectiveness");
        }

        // Check for defensive vulnerabilities
        if (request.Endurance == 0)
        {
            result.AddWarning("Having 0 Endurance may make your character vulnerable in combat");
        }

        // Check for skill check limitations
        if (request.Intelligence == 0 && request.Awareness == 0)
        {
            result.AddWarning("Having 0 in both Intelligence and Awareness may limit non-combat capabilities");
        }
    }
}