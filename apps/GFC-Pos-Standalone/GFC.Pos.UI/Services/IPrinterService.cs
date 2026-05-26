namespace GFC.Pos.UI.Services;

public interface IPrinterService
{
    Task<bool> PrintReceiptAsync(string content);
    Task<bool> PrintRawDataAsync(byte[] data);
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
        var lines = clean.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            
            // Format double-column flex-like rows
            if (line.Contains("justify-content:space-between") || line.Contains("justify-content: space-between") || line.Contains("justify-content:between") || line.Contains("justify-content: between"))
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(line, @"<span[^>]*>([\s\S]*?)</span>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (matches.Count >= 2)
                {
                    var left = StripTags(matches[0].Groups[1].Value).Trim();
                    var right = StripTags(matches[1].Groups[1].Value).Trim();

                    // Standard thermal print width is 40 characters
                    int spaceCount = 40 - left.Length - right.Length;
                    if (spaceCount > 0)
                    {
                        lines[i] = left + new string(' ', spaceCount) + right;
                    }
                    else
                    {
                        lines[i] = left + " " + right;
                    }
                    continue;
                }
            }

            // Format headers (center aligned h1, h2, h3, h4)
            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"<h[1-4][^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase) || line.Contains("text-align:center") || line.Contains("text-align: center") || line.Contains("class=\"center\"") || line.Contains("class='center'"))
            {
                var text = StripTags(line).Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    int spaceCount = (40 - text.Length) / 2;
                    if (spaceCount > 0)
                    {
                        lines[i] = new string(' ', spaceCount) + text.ToUpper();
                    }
                    else
                    {
                        lines[i] = text.ToUpper();
                    }
                    continue;
                }
            }

            // Inline formatting tags replacement
            line = line.Replace("<hr/>", "----------------------------------------");
            line = line.Replace("<hr>", "----------------------------------------");
            line = line.Replace("<br/>", "\n");
            line = line.Replace("<br>", "\n");
            line = line.Replace("</div>", "\n");
            line = line.Replace("</p>", "\n");
            line = line.Replace("</li>", "\n");

            lines[i] = StripTags(line);
        }

        var result = string.Join("\n", lines);

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

