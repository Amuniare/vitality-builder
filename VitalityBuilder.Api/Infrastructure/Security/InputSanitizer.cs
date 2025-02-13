using System.Text.RegularExpressions;

namespace VitalityBuilder.Api.Infrastructure.Security;

/// <summary>
/// Provides input sanitization for user-provided data
/// </summary>
public static class InputSanitizer
{
    // Common patterns for validation
    private static readonly Regex ValidNamePattern = new(@"^[a-zA-Z0-9\s\-']{1,100}$", RegexOptions.Compiled);
    private static readonly Regex ValidDescriptionPattern = new(@"^[a-zA-Z0-9\s\-'.,!?()]{1,1000}$", RegexOptions.Compiled);
    private static readonly Regex ScriptPattern = new(@"<script.*?>.*?</script>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
    private static readonly Regex HtmlTagPattern = new(@"<[^>]*>", RegexOptions.Compiled);
    private static readonly Regex SqlInjectionPattern = new(@"(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Sanitizes a character name
    /// </summary>
    public static string SanitizeName(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Trim and normalize spaces
        var sanitized = NormalizeWhitespace(input);

        // Ensure it matches our valid pattern
        if (!ValidNamePattern.IsMatch(sanitized))
        {
            // Remove any invalid characters
            sanitized = Regex.Replace(sanitized, @"[^a-zA-Z0-9\s\-']", "");
        }

        return sanitized.Length > 100 ? sanitized[..100] : sanitized;
    }

    /// <summary>
    /// Sanitizes a description or longer text field
    /// </summary>
    public static string SanitizeDescription(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove potential XSS attempts
        var sanitized = RemoveScriptTags(input);

        // Remove HTML tags
        sanitized = RemoveHtmlTags(sanitized);

        // Normalize whitespace
        sanitized = NormalizeWhitespace(sanitized);

        // Ensure it matches our valid pattern
        if (!ValidDescriptionPattern.IsMatch(sanitized))
        {
            // Remove any invalid characters
            sanitized = Regex.Replace(sanitized, @"[^a-zA-Z0-9\s\-'.,!?()]", "");
        }

        return sanitized.Length > 1000 ? sanitized[..1000] : sanitized;
    }

    /// <summary>
    /// Sanitizes input that will be used in SQL queries
    /// </summary>
    public static string SanitizeSqlInput(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Check for potential SQL injection attempts
        if (SqlInjectionPattern.IsMatch(input))
        {
            throw new ArgumentException("Potential SQL injection detected");
        }

        // Replace single quotes with double single quotes
        return input.Replace("'", "''");
    }

    /// <summary>
    /// Validates and sanitizes an enumeration value
    /// </summary>
    public static string SanitizeEnum<T>(string? input) where T : Enum
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove any non-alphanumeric characters
        var sanitized = Regex.Replace(input, @"[^a-zA-Z0-9]", "");

        // Verify it's a valid enum value
        if (Enum.TryParse(typeof(T), sanitized, true, out _))
        {
            return sanitized;
        }

        throw new ArgumentException($"Invalid value for enum type {typeof(T).Name}");
    }

    /// <summary>
    /// Normalizes whitespace in a string
    /// </summary>
    private static string NormalizeWhitespace(string input)
    {
        // Replace multiple spaces with a single space
        var normalized = Regex.Replace(input.Trim(), @"\s+", " ");

        // Remove any control characters
        return Regex.Replace(normalized, @"[\x00-\x1F\x7F]", "");
    }

    /// <summary>
    /// Removes script tags from input
    /// </summary>
    private static string RemoveScriptTags(string input)
    {
        // Remove script tags and their contents
        return ScriptPattern.Replace(input, "");
    }

    /// <summary>
    /// Removes HTML tags from input
    /// </summary>
    private static string RemoveHtmlTags(string input)
    {
        // Remove HTML tags but keep their contents
        return HtmlTagPattern.Replace(input, "");
    }

    /// <summary>
    /// Checks if a string might contain malicious content
    /// </summary>
    public static bool ContainsSuspiciousContent(string input)
    {
        // Check for script tags
        if (ScriptPattern.IsMatch(input))
        {
            return true;
        }

        // Check for SQL injection attempts
        if (SqlInjectionPattern.IsMatch(input))
        {
            return true;
        }

        // Check for potential shell commands
        if (input.Contains('&') || input.Contains('|') || input.Contains(';'))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Validates and sanitizes a file path
    /// </summary>
    public static string SanitizeFilePath(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove any directory traversal attempts
        var sanitized = input.Replace("..", "");

        // Remove any characters that aren't valid in file paths
        sanitized = Regex.Replace(sanitized, @"[^\w\-./\\]", "");

        return sanitized;
    }
}