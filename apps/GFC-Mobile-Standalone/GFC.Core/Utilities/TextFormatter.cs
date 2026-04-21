using System.Text.RegularExpressions;

namespace GFC.Core.Utilities;

/// <summary>
/// Utility class for normalizing and formatting text data for member records.
/// </summary>
public static class TextFormatter
{
    /// <summary>
    /// Converts text to smart multi-word title case (e.g., "NEW YORK" -> "New York").
    /// </summary>
    public static string ToTitleCaseWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return string.Join(" ",
            input.Trim()
                 .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                 .Select(word =>
                 {
                     if (word.Length == 1)
                         return word.ToUpper();

                     return char.ToUpper(word[0]) + word.Substring(1).ToLower();
                 }));
    }

    /// <summary>
    /// Converts text to smart person-name casing, handling prefixes like Mc, Mac, O', and common particles.
    /// </summary>
    public static string ToSmartPersonName(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim();

        var lower = input.ToLower();
        var parts = lower.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        for (int i = 0; i < parts.Count; i++)
        {
            var word = parts[i];

            // Handle O' prefix (O'Brien)
            if (word.StartsWith("o'") && word.Length > 2)
            {
                var rest = word.Substring(2);
                parts[i] = "O'" + (rest.Length > 0 ? char.ToUpper(rest[0]) + (rest.Length > 1 ? rest.Substring(1) : "") : "");
                continue;
            }

            // Handle Mc / Mac prefixes
            if (word.StartsWith("mc") && word.Length > 2)
            {
                // McDonald
                parts[i] = "Mc" + char.ToUpper(word[2]) + (word.Length > 3 ? word.Substring(3) : "");
                continue;
            }

            if (word.StartsWith("mac") && word.Length > 3)
            {
                // MacArthur
                parts[i] = "Mac" + char.ToUpper(word[3]) + (word.Length > 4 ? word.Substring(4) : "");
                continue;
            }

            // Handle particles like de, da, di, van, von, st, st.
            var particlesLower = new HashSet<string> { "de", "da", "di", "van", "von", "st", "st." };
            if (particlesLower.Contains(word))
            {
                parts[i] = char.ToUpper(word[0]) + (word.Length > 1 ? word.Substring(1).ToLower() : "");
                continue;
            }

            // Default: simple title case
            if (word.Length == 1)
            {
                parts[i] = word.ToUpper();
            }
            else
            {
                parts[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Normalizes state to 2-letter uppercase (e.g., "california" -> "CA").
    /// </summary>
    public static string NormalizeState(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim().ToUpper();

        if (input.Length > 2)
            input = input.Substring(0, 2);

        return input;
    }

    /// <summary>
    /// Normalizes postal code to US format (5-digit or ZIP+4: 12345 or 12345-6789).
    /// Returns empty string if invalid format.
    /// </summary>
    public static string NormalizePostalCode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim();

        // Accept 12345 or 12345-6789
        if (Regex.IsMatch(input, @"^\d{5}(-\d{4})?$"))
            return input;

        return string.Empty; // Clear invalid format
    }

    /// <summary>
    /// Normalizes suffix to standard format (Jr, Sr, II, III, IV).
    /// </summary>
    public static string NormalizeSuffix(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        input = input.Trim().Trim('.').ToUpper();

        return input switch
        {
            "JR" => "Jr",
            "SR" => "Sr",
            "II" => "II",
            "III" => "III",
            "IV" => "IV",
            _ => ToSmartPersonName(input) // fallback
        };
    }

    /// <summary>
    /// Normalizes phone number to format ###-###-#### if 10 digits are found.
    /// Returns original input if not 10 digits.
    /// </summary>
    public static string NormalizePhone(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var digits = new string(input.Where(char.IsDigit).ToArray());

        if (digits.Length != 10)
            return input.Trim(); // fallback: return as-is if not 10 digits

        return $"{digits.Substring(0, 3)}-{digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
    }
}



