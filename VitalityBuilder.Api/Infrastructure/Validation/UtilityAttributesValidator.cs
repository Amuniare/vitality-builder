using FluentValidation;
using VitalityBuilder.Api.Domain.Constants;
using VitalityBuilder.Api.Domain.Dtos.Attributes;

namespace VitalityBuilder.Api.Infrastructure.Validation;

public class UtilityAttributesValidator : AbstractValidator<UtilityAttributesDto>
{
    private readonly int _characterTier;

    public UtilityAttributesValidator(int characterTier)
    {
        _characterTier = characterTier;

        // Validate Awareness
        RuleFor(x => x.Awareness)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Awareness must be between 0 and {_characterTier}");

        // Validate Communication
        RuleFor(x => x.Communication)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Communication must be between 0 and {_characterTier}");

        // Validate Intelligence
        RuleFor(x => x.Intelligence)
            .InclusiveBetween(0, _characterTier)
            .WithMessage($"Intelligence must be between 0 and {_characterTier}");

        // Validate Total Points
        RuleFor(x => x)
            .Must(ValidateTotalPoints)
            .WithMessage($"Total utility attributes cannot exceed {_characterTier} points");

        // Validate Available Points
        RuleFor(x => x)
            .Must(ValidatePointsSpent)
            .WithMessage(x => $"Cannot spend more than {x.AvailablePoints} points");

        // Expertise Validations
        RuleFor(x => x)
            .Must(ValidateExpertiseRequirements)
            .WithMessage("Utility attributes do not meet expertise requirements");

        // Feature Validations
        RuleFor(x => x)
            .Must(ValidateFeatureRequirements)
            .WithMessage("Utility attributes do not meet feature requirements");

        // Context-specific validation rules
        When(x => x.Values.SkillBonuses.Any(), () =>
        {
            RuleFor(x => x)
                .Must(ValidateSkillBonusRequirements)
                .WithMessage("Skill bonus requirements not met");
        });
    }

    private bool ValidateTotalPoints(UtilityAttributesDto attributes)
    {
        return attributes.TotalPoints <= _characterTier;
    }

    private bool ValidatePointsSpent(UtilityAttributesDto attributes)
    {
        return attributes.TotalPoints <= attributes.AvailablePoints;
    }

    private bool ValidateExpertiseRequirements(UtilityAttributesDto attributes)
    {
        foreach (var skillBonus in attributes.Values.SkillBonuses)
        {
            // Get the required attribute value for this expertise
            var requiredValue = GetRequiredAttributeValue(skillBonus.AttributeName);
            var actualValue = GetAttributeValue(attributes, skillBonus.AttributeName);

            if (actualValue < requiredValue)
            {
                return false;
            }
        }
        return true;
    }

    private bool ValidateFeatureRequirements(UtilityAttributesDto attributes)
    {
        // Add specific feature validation logic
        // This would check if attributes meet minimum requirements for chosen features
        return true;
    }

    private bool ValidateSkillBonusRequirements(UtilityAttributesDto attributes)
    {
        // Add specific skill bonus validation logic
        // This would ensure skill bonuses don't exceed game rules
        return true;
    }

    private int GetRequiredAttributeValue(string attributeName)
    {
        // Define minimum attribute requirements for expertise
        return attributeName.ToLower() switch
        {
            "awareness" => 2,
            "communication" => 2,
            "intelligence" => 2,
            _ => throw new ArgumentException($"Unknown attribute: {attributeName}")
        };
    }

    private int GetAttributeValue(UtilityAttributesDto attributes, string attributeName)
    {
        return attributeName.ToLower() switch
        {
            "awareness" => attributes.Awareness,
            "communication" => attributes.Communication,
            "intelligence" => attributes.Intelligence,
            _ => throw new ArgumentException($"Unknown attribute: {attributeName}")
        };
    }

    /// <summary>
    /// Creates utility attribute warnings without failing validation
    /// </summary>
    public IEnumerable<string> GenerateWarnings(UtilityAttributesDto attributes)
    {
        var warnings = new List<string>();

        // Check for unspent points
        if (attributes.RemainingPoints > 0)
        {
            warnings.Add($"You have {attributes.RemainingPoints} unspent utility attribute points");
        }

        // Check for potential skill gaps
        if (attributes.Awareness == 0)
        {
            warnings.Add("Having 0 Awareness may make your character vulnerable to surprises and reduce initiative");
        }

        if (attributes.Communication == 0)
        {
            warnings.Add("Having 0 Communication may limit your character's social interactions");
        }

        if (attributes.Intelligence == 0)
        {
            warnings.Add("Having 0 Intelligence may limit your character's problem-solving capabilities");
        }

        // Check for specialized build warnings
        if (attributes.Awareness >= _characterTier &&
            attributes.Communication == 0 &&
            attributes.Intelligence == 0)
        {
            warnings.Add("Highly specialized in Awareness - consider diversifying for better utility");
        }

        if (attributes.Communication >= _characterTier &&
            attributes.Awareness == 0 &&
            attributes.Intelligence == 0)
        {
            warnings.Add("Highly specialized in Communication - consider diversifying for better utility");
        }

        if (attributes.Intelligence >= _characterTier &&
            attributes.Awareness == 0 &&
            attributes.Communication == 0)
        {
            warnings.Add("Highly specialized in Intelligence - consider diversifying for better utility");
        }

        // Check for expertise-related warnings
        foreach (var skillBonus in attributes.Values.SkillBonuses)
        {
            var attributeValue = GetAttributeValue(attributes, skillBonus.AttributeName);
            if (attributeValue == 1 && skillBonus.ExpertiseBonus > 0)
            {
                warnings.Add($"Low {skillBonus.AttributeName} value may limit the effectiveness of related expertise");
            }
        }

        // Dynamic Entry warnings
        if (attributes.Intelligence == 1)
        {
            warnings.Add("Minimum Intelligence limits Dynamic Entry to only one ally");
        }

        if (attributes.Communication == 1)
        {
            warnings.Add("Minimum Communication limits Dynamic Entry range");
        }

        return warnings;
    }
}