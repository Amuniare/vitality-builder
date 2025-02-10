using FluentValidation;
using VitalityBuilder.Api.Models.DTOs;

namespace VitalityBuilder.Api.Models.Validations;

public class CreateCharacterDtoValidator : AbstractValidator<CreateCharacterDto>
{
    public CreateCharacterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Character name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Tier)
            .InclusiveBetween(1, 10)
            .WithMessage("Tier must be between 1 and 10");

        RuleFor(x => x.CombatAttributes)
            .NotNull()
            .SetValidator(new CombatAttributesDtoValidator());

        RuleFor(x => x.UtilityAttributes)
            .NotNull()
            .SetValidator(new UtilityAttributesDtoValidator());
    }
}

public class CombatAttributesDtoValidator : AbstractValidator<CombatAttributesDto>
{
    public CombatAttributesDtoValidator()
    {
        RuleFor(x => x.Focus)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Focus cannot be negative");

        RuleFor(x => x.Power)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Power cannot be negative");

        RuleFor(x => x.Mobility)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mobility cannot be negative");

        RuleFor(x => x.Endurance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Endurance cannot be negative");

        RuleFor(x => x.Total)
            .Must((dto, total) => total >= 0)
            .WithMessage("Total combat attributes cannot be negative");
    }
}

public class UtilityAttributesDtoValidator : AbstractValidator<UtilityAttributesDto>
{
    public UtilityAttributesDtoValidator()
    {
        RuleFor(x => x.Awareness)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Awareness cannot be negative");

        RuleFor(x => x.Communication)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Communication cannot be negative");

        RuleFor(x => x.Intelligence)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Intelligence cannot be negative");

        RuleFor(x => x.Total)
            .Must((dto, total) => total >= 0)
            .WithMessage("Total utility attributes cannot be negative");
    }
}

public class CharacterArchetypesDtoValidator : AbstractValidator<CharacterArchetypesDto>
{
    public CharacterArchetypesDtoValidator()
    {
        RuleFor(x => x.MovementArchetype)
            .NotNull()
            .SetValidator(new MovementArchetypeDtoValidator());

        RuleFor(x => x.AttackTypeArchetype)
            .NotNull()
            .SetValidator(new AttackTypeArchetypeDtoValidator());

        RuleFor(x => x.EffectTypeArchetype)
            .NotNull()
            .SetValidator(new EffectTypeArchetypeDtoValidator());

        RuleFor(x => x.UniqueAbilityArchetype)
            .NotNull()
            .SetValidator(new UniqueAbilityArchetypeDtoValidator());

        RuleFor(x => x.SpecialAttackArchetype)
            .NotNull()
            .SetValidator(new SpecialAttackArchetypeDtoValidator());

        RuleFor(x => x.UtilityArchetype)
            .NotNull()
            .SetValidator(new UtilityArchetypeDtoValidator());
    }
}

public class MovementArchetypeDtoValidator : AbstractValidator<MovementArchetypeDto>
{
    public MovementArchetypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid movement type specified");

        RuleFor(x => x.SpeedBonusByTier)
            .NotNull()
            .Must(x => x.All(kvp => kvp.Key >= 1 && kvp.Key <= 10))
            .WithMessage("Tier bonuses must be between 1 and 10")
            .Must(x => x.All(kvp => kvp.Value >= 0))
            .WithMessage("Speed bonuses must be non-negative");

        RuleFor(x => x.MovementMultiplier)
            .InclusiveBetween(0.5f, 2.0f)
            .WithMessage("Movement multiplier must be between 0.5 and 2.0");
    }
}