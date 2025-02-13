namespace VitalityBuilder.Domain.Errors;

/// <summary>
/// Represents the result of a validation operation
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Whether the validation passed
    /// </summary>
    public bool IsValid => !Errors.Any();

    /// <summary>
    /// Collection of validation errors
    /// </summary>
    public ICollection<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Collection of validation warnings
    /// </summary>
    public ICollection<string> Warnings { get; set; } = new List<string>();

    /// <summary>
    /// Additional validation data
    /// </summary>
    public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Creates a successful validation result
    /// </summary>
    public static ValidationResult Success()
    {
        return new ValidationResult();
    }

    /// <summary>
    /// Creates a failed validation result with errors
    /// </summary>
    public static ValidationResult Failure(params string[] errors)
    {
        return new ValidationResult
        {
            Errors = errors.ToList()
        };
    }

    /// <summary>
    /// Creates a successful validation result with warnings
    /// </summary>
    public static ValidationResult WithWarnings(params string[] warnings)
    {
        return new ValidationResult
        {
            Warnings = warnings.ToList()
        };
    }

    /// <summary>
    /// Adds an error to the validation result
    /// </summary>
    public ValidationResult AddError(string error)
    {
        Errors.Add(error);
        return this;
    }

    /// <summary>
    /// Adds multiple errors to the validation result
    /// </summary>
    public ValidationResult AddErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors)
        {
            Errors.Add(error);
        }
        return this;
    }

    /// <summary>
    /// Adds a warning to the validation result
    /// </summary>
    public ValidationResult AddWarning(string warning)
    {
        Warnings.Add(warning);
        return this;
    }

    /// <summary>
    /// Adds multiple warnings to the validation result
    /// </summary>
    public ValidationResult AddWarnings(IEnumerable<string> warnings)
    {
        foreach (var warning in warnings)
        {
            Warnings.Add(warning);
        }
        return this;
    }

    /// <summary>
    /// Adds additional data to the validation result
    /// </summary>
    public ValidationResult AddData(string key, object value)
    {
        Data[key] = value;
        return this;
    }

    /// <summary>
    /// Combines multiple validation results into one
    /// </summary>
    public static ValidationResult Combine(params ValidationResult[] results)
    {
        var combined = new ValidationResult();

        foreach (var result in results)
        {
            combined.AddErrors(result.Errors);
            combined.AddWarnings(result.Warnings);
            foreach (var kvp in result.Data)
            {
                combined.Data[kvp.Key] = kvp.Value;
            }
        }

        return combined;
    }

    /// <summary>
    /// Creates an attribute validation result
    /// </summary>
    public static ValidationResult FromAttributeValidation(
        bool isValid, 
        int availablePoints, 
        int spentPoints, 
        int maxPerAttribute)
    {
        var result = new ValidationResult();

        if (!isValid)
        {
            result.AddError($"Attributes exceed maximum value of {maxPerAttribute}");
        }

        if (spentPoints > availablePoints)
        {
            result.AddError($"Insufficient points: spent {spentPoints}, available {availablePoints}");
        }
        else if (spentPoints < availablePoints)
        {
            result.AddWarning($"Unspent points: {availablePoints - spentPoints} remaining");
        }

        result.AddData("AvailablePoints", availablePoints);
        result.AddData("SpentPoints", spentPoints);
        result.AddData("RemainingPoints", availablePoints - spentPoints);

        return result;
    }

    /// <summary>
    /// Creates an archetype validation result
    /// </summary>
    public static ValidationResult FromArchetypeValidation(
        bool hasRequiredArchetypes,
        bool isCompatible,
        IEnumerable<string>? incompatibilities = null)
    {
        var result = new ValidationResult();

        if (!hasRequiredArchetypes)
        {
            result.AddError("All archetypes must be selected");
        }

        if (!isCompatible)
        {
            result.AddError("Selected archetypes are not compatible");
            if (incompatibilities != null)
            {
                foreach (var incompatibility in incompatibilities)
                {
                    result.AddError(incompatibility);
                }
            }
        }

        return result;
    }
}