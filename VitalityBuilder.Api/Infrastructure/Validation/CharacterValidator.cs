using FluentValidation;
using VitalityBuilder.Api.Domain.Constants;
using VitalityBuilder.Api.Domain.Dtos.Character;

namespace VitalityBuilder.Api.Infrastructure.Validation;

public class CharacterValidator : AbstractValidator<CharacterBasicDto>
{
    public CharacterValidator()
    {
        // Name Validation
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Character name is required")
            .Length(1, 100)
            .WithMessage("Name must be between 1 and 100 characters")
            .Must(name => !name.Contains("  "))
            .WithMessage("Name cannot contain consecutive spaces")
            .Matches("^[a-zA-Z0-9\\s\\-']*$")
            .WithMessage("Name can only contain letters, numbers, spaces, hyphens, and apostrophes");

        // Tier Validation
        RuleFor(x => x.Tier)
            .InclusiveBetween(GameRuleConstants.MinimumTier, GameRuleConstants.MaximumTier)
            .WithMessage($"Tier must be between {GameRuleConstants.MinimumTier} and {GameRuleConstants.MaximumTier}");

        // Combat Attributes
        RuleFor(x => x)
            .Must(ValidateCombatAttributeTotal)
            .WithMessage(x => $"Total combat attributes cannot exceed {x.Tier * 2} points");

        RuleFor(x => x.Focus)
            .Must((character, focus) => focus <= character.Tier)
            .WithMessage(x => $"Focus cannot exceed character tier ({x.Tier})");

        RuleFor(x => x.Power)
            .Must((character, power) => power <= character.Tier)
            .WithMessage(x => $"Power cannot exceed character tier ({x.Tier})");

        RuleFor(x => x.Mobility)
            .Must((character, mobility) => mobility <= character.Tier)
            .WithMessage(x => $"Mobility cannot exceed character tier ({x.Tier})");

        RuleFor(x => x.Endurance)
            .Must((character, endurance) => endurance <= character.Tier)
            .WithMessage(x => $"Endurance cannot exceed character tier ({x.Tier})");

        // Utility Attributes
        RuleFor(x => x)
            .Must(ValidateUtilityAttributeTotal)
            .WithMessage(x => $"Total utility attributes cannot exceed {x.Tier} points");

        RuleFor(x => x.Awareness)
            .Must((character, awareness) => awareness <= character.Tier)
            .WithMessage(x => $"Awareness cannot exceed character tier ({x.Tier})");

        RuleFor(x => x.Communication)
            .Must((character, communication) => communication <= character.Tier)
            .WithMessage(x => $"Communication cannot exceed character tier ({x.Tier})");

        RuleFor(x => x.Intelligence)
            .Must((character, intelligence) => intelligence <= character.Tier)
            .WithMessage(x => $"Intelligence cannot exceed character tier ({x.Tier})");

        // Archetype Validation
        RuleFor(x => x.MovementArchetype)
            .NotEmpty()
            .WithMessage("Movement archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid movement archetype selected");

        RuleFor(x => x.AttackArchetype)
            .NotEmpty()
            .WithMessage("Attack archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid attack archetype selected");

        RuleFor(x => x.EffectArchetype)
            .NotEmpty()
            .WithMessage("Effect archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid effect archetype selected");

        RuleFor(x => x.UniqueAbilityArchetype)
            .NotEmpty()
            .WithMessage("Unique ability archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid unique ability archetype selected");

        RuleFor(x => x.SpecialAttackArchetype)
            .NotEmpty()
            .WithMessage("Special attack archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid special attack archetype selected");

        RuleFor(x => x.UtilityArchetype)
            .NotEmpty()
            .WithMessage("Utility archetype is required")
            .Must(BeValidArchetype)
            .WithMessage("Invalid utility archetype selected");
    }

    private bool ValidateCombatAttributeTotal(CharacterBasicDto character)
    {
        var total = character.Focus + character.Power + 
                   character.Mobility + character.Endurance;
        return total <= character.Tier * 2;
    }

    private bool ValidateUtilityAttributeTotal(CharacterBasicDto character)
    {
        var total = character.Awareness + character.Communication + 
                   character.Intelligence;
        return total <= character.Tier;
    }

    private bool BeValidArchetype(string archetype)
    {
        // Add specific archetype validation logic based on your enums
        return !string.IsNullOrWhiteSpace(archetype) && 
               archetype.All(c => char.IsLetterOrDigit(c) || c == '_');
    }
}