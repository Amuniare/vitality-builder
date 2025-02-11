using FluentValidation;
using VitalityBuilder.Api.Models.DTOs;
using VitalityBuilder.Api.Models.Archetypes;

namespace VitalityBuilder.Api.Models.Validations;

public class AttackTypeArchetypeDtoValidator : AbstractValidator<AttackTypeArchetypeDto>
{
    public AttackTypeArchetypeDtoValidator()
    {
        RuleFor(static x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(static x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid attack type category specified");

        RuleFor(static x => x.AccuracyPenalty)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)AttackTypeArchetypeCategory.AOESpecialist)
            .WithMessage("AOE Specialist must have a non-negative accuracy penalty");

        RuleFor(static x => x.EffectPenalty)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)AttackTypeArchetypeCategory.DirectSpecialist)
            .WithMessage("Direct Specialist must have a non-negative effect penalty");
    }
}

public class EffectTypeArchetypeDtoValidator : AbstractValidator<EffectTypeArchetypeDto>
{
    public EffectTypeArchetypeDtoValidator()
    {
        RuleFor(static x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(static x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid effect type category specified");

        RuleFor(static x => x.DamagePenalty)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)EffectTypeCategory.HybridSpecialist || (int)x.Category == (int)EffectTypeCategory.CrowdControl)
            .WithMessage("Damage penalty must be non-negative for Hybrid Specialist and Crowd Control");

        RuleFor(static x => x.ConditionPenalty)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)EffectTypeCategory.HybridSpecialist)
            .WithMessage("Condition penalty must be non-negative for Hybrid Specialist");
    }
}

public class UniqueAbilityArchetypeDtoValidator : AbstractValidator<UniqueAbilityArchetypeDto>
{
    public UniqueAbilityArchetypeDtoValidator()
    {
        RuleFor(static x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(static x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid unique ability category specified");

        RuleFor(static x => x.ExtraQuickActions)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)UniqueAbilityCategory.VersatileMaster)
            .WithMessage("Extra quick actions must be non-negative for Versatile Master");

        RuleFor(static x => x.ExtraPointPool)
            .GreaterThanOrEqualTo(0)
            .When(static x => (int)x.Category == (int)UniqueAbilityCategory.Extraordinary)
            .WithMessage("Extra point pool must be non-negative for Extraordinary");

        RuleFor(static x => x.StatBonuses)
            .NotNull()
            .Must(static x => x.All(static kvp => kvp.Value >= 0))
            .WithMessage("All stat bonuses must be non-negative");
    }
}

public class SpecialAttackArchetypeDtoValidator : AbstractValidator<SpecialAttackArchetypeDto>
{
    public SpecialAttackArchetypeDtoValidator()
    {
        RuleFor(static x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(static x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid special attack category specified");

        RuleFor(static x => x.BasePoints)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base points must be non-negative");

        RuleFor(static x => x.MaxSpecialAttacks)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Maximum special attacks must be at least 1");

        RuleFor(static x => x.LimitPointMultiplier)
            .GreaterThanOrEqualTo(0)
            .When(static x => x.CanTakeLimits)
            .WithMessage("Limit point multiplier must be non-negative when limits are allowed");

        RuleFor(static x => x.RequiredLimits)
            .NotNull()
            .Must(static x => !x.Any(string.IsNullOrWhiteSpace))
            .When(static x => (int)x.Category == (int)SpecialAttackCategory.Specialist)
            .WithMessage("Specialist archetype must have at least one required limit");
    }
}

public class UtilityArchetypeDtoValidator : AbstractValidator<UtilityArchetypeDto>
{
    public UtilityArchetypeDtoValidator()
    {
        RuleFor(static x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(static x => x.Category)
            .IsInEnum()
            .WithMessage("Invalid utility category specified");

        RuleFor(static x => x.BaseUtilityPool)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base utility pool must be non-negative");

        RuleFor(static x => x.TierBonusMultiplier)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tier bonus multiplier must be non-negative");

        RuleFor(static x => x.Restrictions)
            .NotNull()
            .Must(static x => !x.Any(string.IsNullOrWhiteSpace))
            .When(static x => (int)x.Category == (int)UtilityCategory.Specialized || (int)x.Category == (int)UtilityCategory.JackOfAllTrades)
            .WithMessage("Specialized and Jack of All Trades archetypes must have valid restrictions");
    }
}