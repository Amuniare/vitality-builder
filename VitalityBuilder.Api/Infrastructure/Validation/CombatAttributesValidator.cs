using FluentValidation;
using VitalityBuilder.Api.Domain.Constants;
using VitalityBuilder.Api.Domain.Dtos.Attributes;

namespace VitalityBuilder.Api.Infrastructure.Validation;

public class CombatAttributesValidator : AbstractValidator<CombatAttributesDto>
{
    private readonly int _characterTier;

    public CombatAttributesValidator(int characterTier)
    {
        _characterTier = characterTier;

        // Validate Focus
        RuleFor(x => x.Focus)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Focus must be between 0 and {_characterTier}");

        // Validate Power
        RuleFor(x => x.Power)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Power must be between 0 and {_characterTier}");

        // Validate Mobility
        RuleFor(x => x.Mobility)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Mobility must be between 0 and {_characterTier}");

        // Validate Endurance
        RuleFor(x => x.Endurance)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Endurance must be between 0 and {_characterTier}");

        // Validate Total Points
        RuleFor(x => x)
            .Must(ValidateTotalPoints)
            .WithMessage($"Total combat attributes cannot exceed {_characterTier * 2} points");

        // Validate Available Points
        RuleFor(x => x)
            .Must(ValidatePointsSpent)
            .WithMessage(x => $"Cannot spend more than {x.AvailablePoints} points");

        // Custom Validations
        RuleFor(x => x)
            .Must(ValidateArchetypeRequirements)
            .WithMessage("Combat attributes do not meet archetype requirements")
            .Must(ValidateSpecialAttackRequirements)
            .WithMessage("Combat attributes do not meet special attack requirements");

        // Context-specific validation rules
        When(x => x.Focus > 0 && x.Power > 0, () =>
        {
            RuleFor(x => x)
                .Must(ValidateHybridBuildRequirements)
                .WithMessage("Hybrid build requirements not met");
        });
    }

    private bool ValidateTotalPoints(CombatAttributesDto attributes)
    {
        return attributes.TotalPoints <= _characterTier * 2;
    }

    private bool ValidatePointsSpent(CombatAttributesDto attributes)
    {
        return attributes.TotalPoints <= attributes.AvailablePoints;
    }

    private bool ValidateArchetypeRequirements(CombatAttributesDto attributes)
    {
        // Add specific archetype validation logic
        // This would check any minimum attribute requirements based on selected archetypes
        return true;
    }

    private bool ValidateSpecialAttackRequirements(CombatAttributesDto attributes)
    {
        // Add specific special attack validation logic
        // This would check if attributes meet minimum requirements for chosen special attacks
        return true;
    }

    private bool ValidateHybridBuildRequirements(CombatAttributesDto attributes)
    {
        // Add specific hybrid build validation logic
        // This would enforce any special rules for characters using both Focus and Power
        return true;
    }

    /// <summary>
    /// Creates combat attribute warnings without failing validation
    /// </summary>
    public IEnumerable<string> GenerateWarnings(CombatAttributesDto attributes)
    {
        var warnings = new List<string>();

        // Check for unspent points
        if (attributes.RemainingPoints > 0)
        {
            warnings.Add($"You have {attributes.RemainingPoints} unspent combat attribute points");
        }

        // Check for potentially inefficient allocations
        if (attributes.Focus > 0 && attributes.Power > 0 && attributes.Mobility > 0)
        {
            warnings.Add("Spreading points across Focus, Power, and Mobility may reduce combat effectiveness");
        }

        // Check for defensive vulnerabilities
        if (attributes.Endurance == 0)
        {
            warnings.Add("Having 0 Endurance may make your character vulnerable in combat");
        }

        // Check for mobility issues
        if (attributes.Mobility == 0)
        {
            warnings.Add("Having 0 Mobility may limit your tactical options in combat");
        }

        return warnings;
    }
}