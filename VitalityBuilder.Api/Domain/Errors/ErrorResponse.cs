namespace VitalityBuilder.Api.Domain.Errors;

/// <summary>
/// Standardized error response for API endpoints
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Type of error that occurred
    /// </summary>
    public string Type { get; set; } = "Error";

    /// <summary>
    /// Human-readable error message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Unique error code for tracking
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Detailed error information
    /// </summary>
    public ICollection<string> Details { get; set; } = new List<string>();

    /// <summary>
    /// Additional data relevant to the error
    /// </summary>
    public IDictionary<string, object>? Data { get; set; }

    /// <summary>
    /// Creates a validation error response
    /// </summary>
    public static ErrorResponse ValidationError(string message, IEnumerable<string> details)
    {
        return new ErrorResponse
        {
            Type = "ValidationError",
            Message = message,
            Details = details.ToList()
        };
    }

    /// <summary>
    /// Creates a not found error response
    /// </summary>
    public static ErrorResponse NotFound(string resource, string identifier)
    {
        return new ErrorResponse
        {
            Type = "NotFound",
            Message = $"{resource} with identifier {identifier} was not found",
            Code = "404"
        };
    }

    /// <summary>
    /// Creates a business rule violation error response
    /// </summary>
    public static ErrorResponse BusinessRuleViolation(string message, IEnumerable<string> details)
    {
        return new ErrorResponse
        {
            Type = "BusinessRuleViolation",
            Message = message,
            Details = details.ToList()
        };
    }

    /// <summary>
    /// Creates an internal server error response
    /// </summary>
    public static ErrorResponse InternalError(string message, string? code = null)
    {
        return new ErrorResponse
        {
            Type = "InternalError",
            Message = message,
            Code = code ?? "500"
        };
    }

    /// <summary>
    /// Creates an invalid operation error response
    /// </summary>
    public static ErrorResponse InvalidOperation(string message, IEnumerable<string>? details = null)
    {
        return new ErrorResponse
        {
            Type = "InvalidOperation",
            Message = message,
            Details = details?.ToList() ?? new List<string>()
        };
    }
}