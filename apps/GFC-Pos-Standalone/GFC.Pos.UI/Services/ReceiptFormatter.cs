namespace GFC.Pos.UI.Services;

/// <summary>
/// Shared plain-text receipt layout engine for 80mm thermal printers (42-char width).
/// All output lines are strictly 42 characters wide (excluding the trailing \n).
/// </summary>
public static class ReceiptFormatter
{
    public const int Width = 42;

    // ── Dividers ────────────────────────────────────────────────────────────
    public static string Divider(char ch = '-') => new string(ch, Width);
    public static string DashedDivider()  => Divider('-');
    public static string SolidDivider()   => Divider('=');
    public static string DottedDivider()  => Divider('.');

    // ── Single-line helpers ──────────────────────────────────────────────────

    /// <summary>Center text within 42 characters. Text longer than 42 chars is truncated.</summary>
    public static string Center(string text)
    {
        if (text.Length > Width) text = text[..Width];
        int totalPad = Width - text.Length;
        int left = totalPad / 2;
        return new string(' ', left) + text.PadRight(Width - left);
    }

    /// <summary>Left-align text, padded to 42 characters. Text longer than 42 is truncated.</summary>
    public static string Left(string text)
    {
        if (text.Length > Width) text = text[..Width];
        return text.PadRight(Width);
    }

    public static string FormatLine(string label, string value, char padChar = ' ')
    {
        // Ensure value fits first
        if (value.Length > Width) value = value[..Width];

        int maxLabelLen = Width - value.Length - 1; // at least 1 space gap
        if (maxLabelLen < 1) maxLabelLen = 1;
        if (label.Length > maxLabelLen) label = label[..maxLabelLen];

        int spaces = Width - label.Length - value.Length;
        if (spaces < 1) spaces = 1;

        return label + new string(padChar, spaces) + value;
    }

    /// <summary>
    /// Two-column row where the label is indented with a prefix (e.g. "  - label").
    /// </summary>
    public static string FormatIndentedLine(string label, string value, int indent = 2)
    {
        var prefix = new string(' ', indent) + label;
        return FormatLine(prefix, value);
    }

    /// <summary>
    /// Word-wrap a long string into multiple 42-char lines.
    /// Returns an IEnumerable of lines (without trailing \n).
    /// </summary>
    public static IEnumerable<string> WrapText(string text, int indent = 0)
    {
        var maxWidth = Width - indent;
        var prefix = new string(' ', indent);
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currentLine = new System.Text.StringBuilder();

        foreach (var word in words)
        {
            if (currentLine.Length == 0)
            {
                currentLine.Append(word);
            }
            else if (currentLine.Length + 1 + word.Length <= maxWidth)
            {
                currentLine.Append(' ');
                currentLine.Append(word);
            }
            else
            {
                yield return (prefix + currentLine.ToString()).PadRight(Width);
                currentLine.Clear();
                currentLine.Append(word);
            }
        }

        if (currentLine.Length > 0)
            yield return (prefix + currentLine.ToString()).PadRight(Width);
    }

    // ── Receipt section builders ─────────────────────────────────────────────

    /// <summary>Append a centered line followed by \n.</summary>
    public static void AppendCenter(System.Text.StringBuilder sb, string text)
        => sb.Append(Center(text)).Append('\n');

    /// <summary>Append a full-width divider followed by \n.</summary>
    public static void AppendDivider(System.Text.StringBuilder sb, char ch = '-')
        => sb.Append(Divider(ch)).Append('\n');

    /// <summary>Append a two-column label/value row followed by \n.</summary>
    public static void AppendLine(System.Text.StringBuilder sb, string label, string value, char padChar = ' ')
        => sb.Append(FormatLine(label, value, padChar)).Append('\n');

    /// <summary>Append a left-aligned line followed by \n.</summary>
    public static void AppendLeft(System.Text.StringBuilder sb, string text)
        => sb.Append(Left(text)).Append('\n');

    /// <summary>Append a blank line (\n only).</summary>
    public static void AppendBlank(System.Text.StringBuilder sb)
        => sb.Append('\n');

    /// <summary>Append a section header (blank line, label, divider).</summary>
    public static void AppendSectionHeader(System.Text.StringBuilder sb, string title)
    {
        AppendDivider(sb, '-');
        AppendLeft(sb, title.ToUpper());
        AppendDivider(sb, '-');
    }

    /// <summary>Append wrapped text lines.</summary>
    public static void AppendWrapped(System.Text.StringBuilder sb, string text, int indent = 0)
    {
        foreach (var line in WrapText(text, indent))
            sb.Append(line).Append('\n');
    }

    /// <summary>
    /// Build a complete receipt header block:
    ///   [centered] title1
    ///   [centered] title2
    ///   [centered] subtitle (e.g., date)
    ///   [centered] operator line
    ///   dashed divider
    /// </summary>
    public static void AppendReceiptHeader(
        System.Text.StringBuilder sb,
        string title1,
        string title2,
        string dateStr,
        string operatorName,
        bool isReprint = false)
    {
        if (isReprint)
        {
            AppendDivider(sb, '*');
            AppendCenter(sb, "*** REPRINT ***");
            AppendDivider(sb, '*');
        }
        AppendCenter(sb, title1.ToUpper());
        AppendCenter(sb, title2.ToUpper());
        AppendCenter(sb, dateStr);
        AppendCenter(sb, operatorName);
        AppendDivider(sb, '-');
    }
}
