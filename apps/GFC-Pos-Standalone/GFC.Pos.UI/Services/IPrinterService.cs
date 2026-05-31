namespace GFC.Pos.UI.Services;

public interface IPrinterService
{
    Task<bool> PrintReceiptAsync(string content);
    Task<bool> PrintRawDataAsync(byte[] data, global::System.Threading.CancellationToken cancellationToken = default);
    Task<bool> PrintTestAsync();
    Task<bool> KickDrawerAsync();
    Task<List<UsbDeviceDto>> GetConnectedDevicesAsync();
}

public class UsbDeviceDto
{
    public string Name { get; set; } = "";
    public int VendorId { get; set; }
    public int ProductId { get; set; }
    public string VidHex => VendorId.ToString("X4");
    public string PidHex => ProductId.ToString("X4");
}

public static class HtmlToPlainTextConverter
{
    public static string Convert(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";

        // 1. Remove style blocks
        var clean = System.Text.RegularExpressions.Regex.Replace(html, @"<style[^>]*>[\s\S]*?</style>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // 2. Pre-process lines
        var rawLines = clean.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        var processedLines = new System.Collections.Generic.List<string>();

        for (int i = 0; i < rawLines.Length; i++)
        {
            var line = rawLines[i];
            bool isHeader = false;

            // Check if this is a centered header
            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"<h[1-4][^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase) || 
                line.Contains("text-align:center") || 
                line.Contains("text-align: center") || 
                line.Contains("class=\"center\"") || 
                line.Contains("class='center'"))
            {
                isHeader = true;
            }

            // Format double-column flex-like rows
            if (line.Contains("justify-content:space-between") || line.Contains("justify-content: space-between") || line.Contains("justify-content:between") || line.Contains("justify-content: between"))
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(line, @"<span[^>]*>([\s\S]*?)</span>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (matches.Count >= 2)
                {
                    var left = StripTags(matches[0].Groups[1].Value).Trim();
                    var right = StripTags(matches[1].Groups[1].Value).Trim();

                    // Width parameters is strictly 42 characters
                    int spaceCount = 42 - left.Length - right.Length;
                    if (spaceCount > 0)
                    {
                        line = left + new string(' ', spaceCount) + right;
                    }
                    else
                    {
                        // Wrap or truncate left side if too long
                        int maxLeftLen = 42 - right.Length - 1;
                        if (maxLeftLen > 0)
                        {
                            if (left.Length > maxLeftLen)
                            {
                                left = left.Substring(0, maxLeftLen);
                            }
                            spaceCount = 42 - left.Length - right.Length;
                            line = left + new string(' ', spaceCount) + right;
                        }
                        else
                        {
                            line = (left + " " + right).Substring(0, Math.Min(42, left.Length + right.Length + 1)).PadRight(42);
                        }
                    }
                }
            }
            else if (isHeader)
            {
                var text = StripTags(line).Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    // Prepend special header markers that services can parse to change hardware alignments
                    // ESC a 1 (center) will be applied. We prefix with a marker.
                    line = "[ALIGN_CENTER]" + text.ToUpper();
                }
            }
            else
            {
                // Inline formatting tags replacement
                line = System.Text.RegularExpressions.Regex.Replace(line, @"<hr\s*/?>", "------------------------------------------\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                line = System.Text.RegularExpressions.Regex.Replace(line, @"<br\s*/?>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                line = System.Text.RegularExpressions.Regex.Replace(line, @"</(div|p|li|tr|h[1-4])>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                line = StripTags(line);
            }

            // Split line into multiple sub-lines by newlines first
            var subLines = line.Split('\n');
            foreach (var sub in subLines)
            {
                if (sub.StartsWith("[ALIGN_CENTER]"))
                {
                    processedLines.Add(sub);
                }
                else
                {
                    // Enforce the 42 character limit with wrapping/truncating
                    var remainder = sub;
                    while (remainder.Length > 42)
                    {
                        var chunk = remainder.Substring(0, 42);
                        processedLines.Add(chunk.PadRight(42));
                        remainder = remainder.Substring(42);
                    }
                    if (remainder.Length > 0 || string.IsNullOrEmpty(sub))
                    {
                        processedLines.Add(remainder.PadRight(42));
                    }
                }
            }
        }

        var result = string.Join("\n", processedLines);

        // Decode HTML entities
        result = result.Replace("&nbsp;", " ");
        result = result.Replace("&amp;", "&");
        result = result.Replace("&lt;", "<");
        result = result.Replace("&gt;", ">");
        result = result.Replace("&quot;", "\"");
        result = result.Replace("&#x26A0;", "[!]");
        result = result.Replace("&#39;", "'");

        // Clean up excessive newlines
        result = System.Text.RegularExpressions.Regex.Replace(result, @"\n{3,}", "\n\n");

        return result.Trim() + "\n\n\n\n"; // Append paper-cut spacing feed
    }

    private static string StripTags(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, @"<[^>]*>", "");
    }
}

