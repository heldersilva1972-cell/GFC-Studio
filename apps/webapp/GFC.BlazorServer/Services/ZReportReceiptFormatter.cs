using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public static class ZReportReceiptFormatter
    {
        public const int Width = 42;

        public static string Center(string text)
        {
            if (string.IsNullOrEmpty(text)) return new string(' ', Width);
            if (text.Length > Width) text = text[..Width];
            int totalPad = Width - text.Length;
            int left = totalPad / 2;
            return new string(' ', left) + text.PadRight(Width - left);
        }

        public static string Left(string text)
        {
            if (string.IsNullOrEmpty(text)) text = "";
            if (text.Length > Width) text = text[..Width];
            return text.PadRight(Width);
        }

        public static string FormatLine(string label, string value, char padChar = ' ')
        {
            label ??= "";
            value ??= "";
            if (value.Length > Width) value = value[..Width];

            int maxLabelLen = Width - value.Length - 1;
            if (maxLabelLen < 1) maxLabelLen = 1;
            if (label.Length > maxLabelLen) label = label[..maxLabelLen];

            int spaces = Width - label.Length - value.Length;
            if (spaces < 1) spaces = 1;

            return label + new string(padChar, spaces) + value;
        }

        public static IEnumerable<string> WrapText(string text, int indent = 0)
        {
            if (string.IsNullOrWhiteSpace(text)) yield break;
            var maxWidth = Width - indent;
            var prefix = new string(' ', indent);
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var currentLine = new StringBuilder();

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

        public static void AppendCenter(StringBuilder sb, string text) => sb.Append(Center(text)).Append('\n');
        public static void AppendLeft(StringBuilder sb, string text) => sb.Append(Left(text)).Append('\n');
        public static void AppendDivider(StringBuilder sb, char ch = '-') => sb.Append(new string(ch, Width)).Append('\n');
        public static void AppendLine(StringBuilder sb, string label, string value, char padChar = ' ') => sb.Append(FormatLine(label, value, padChar)).Append('\n');
        public static void AppendBlank(StringBuilder sb) => sb.Append('\n');

        public static void AppendSectionHeader(StringBuilder sb, string title)
        {
            AppendDivider(sb, '-');
            AppendLeft(sb, title.ToUpper());
            AppendDivider(sb, '-');
        }

        public class BanquetShiftReportDto
        {
            public int? ActiveEventId { get; set; }
            public string EventName { get; set; } = "";
            public List<decimal> Deposits { get; set; } = new();
            public decimal TotalSpent { get; set; }
            public Dictionary<string, int> ItemSummary { get; set; } = new();
            public Dictionary<string, decimal> ItemTotals { get; set; } = new();
            public string EventType { get; set; } = "RunningTab";
        }

        public static string BuildZReportReceiptText(
            Guid id,
            DateTime timestamp,
            string terminalName,
            string bartenderName,
            decimal cashTotal,
            decimal totalGrossSales,
            decimal tokenCredits,
            string salesSummaryJson,
            string banquetSummaryJson,
            string inventoryPullsJson,
            IEnumerable<LiquorItem>? dbItems = null,
            IEnumerable<PosToken>? dbTokens = null,
            bool isReprint = false,
            string? itemTotalsJson = null)
        {
            var sb = new StringBuilder();

            // Header
            if (isReprint)
            {
                AppendDivider(sb, '*');
                AppendCenter(sb, "*** REPRINT ***");
                AppendDivider(sb, '*');
            }
            AppendCenter(sb, "GFC Z-REPORT");
            AppendCenter(sb, (terminalName ?? "TERMINAL 1").ToUpper());
            AppendCenter(sb, timestamp.ToString("ddd, MMM d yyyy  h:mm tt"));
            AppendCenter(sb, $"Bartender: {bartenderName}");
            AppendDivider(sb, '-');

            // Parse Summaries
            Dictionary<string, int> salesSummary = new();
            try { salesSummary = JsonSerializer.Deserialize<Dictionary<string, int>>(salesSummaryJson ?? "{}") ?? new(); } catch { }

            Dictionary<string, decimal> itemTotals = new();
            try { itemTotals = JsonSerializer.Deserialize<Dictionary<string, decimal>>(itemTotalsJson ?? "{}") ?? new(); } catch { }

            List<BanquetShiftReportDto> banquetSummary = new();
            try { banquetSummary = JsonSerializer.Deserialize<List<BanquetShiftReportDto>>(banquetSummaryJson ?? "[]") ?? new(); } catch { }

            Dictionary<int, int> inventoryPulls = new();
            try { inventoryPulls = JsonSerializer.Deserialize<Dictionary<int, int>>(inventoryPullsJson ?? "{}") ?? new(); } catch { }

            var totalPrepaidDeposits = banquetSummary.Sum(b => b.Deposits.Where(d => d > 0).Sum());
            var totalPrepaidReturned = banquetSummary.Sum(b => b.Deposits.Where(d => d < 0).Sum());

            decimal payoutTotal = 0;
            var payoutLines = new List<(string label, string amount)>();
            var regularSalesSummary = new Dictionary<string, int>(salesSummary);

            int noSaleCount = 0;
            if (regularSalesSummary.ContainsKey("NO SALE (Drawer Open)"))
            {
                noSaleCount = regularSalesSummary["NO SALE (Drawer Open)"];
                regularSalesSummary.Remove("NO SALE (Drawer Open)");
            }

            foreach (var kvp in salesSummary)
            {
                if (kvp.Key.StartsWith("PAYOUT:"))
                {
                    var parts = kvp.Key.Split(':');
                    if (parts.Length >= 4 && int.TryParse(parts[3], out var amountCents))
                    {
                        var cat = parts[1];
                        var desc = parts[2];
                        var amt = amountCents / 100m;
                        payoutTotal += amt;
                        var payLabel = string.IsNullOrEmpty(desc) ? $"  - {cat}" : $"  - {cat} ({desc})";
                        payoutLines.Add((payLabel, $"-{amt:C}"));
                    }
                    regularSalesSummary.Remove(kvp.Key);
                }
            }

            foreach (var b in banquetSummary)
            {
                foreach (var item in b.ItemSummary)
                {
                    if (regularSalesSummary.ContainsKey(item.Key))
                    {
                        regularSalesSummary[item.Key] -= item.Value;
                        if (regularSalesSummary[item.Key] <= 0)
                            regularSalesSummary.Remove(item.Key);
                    }
                }
            }

            var depositKeys = regularSalesSummary.Keys
                .Where(k => k.StartsWith("TAB DEPOSIT:") || k.StartsWith("INITIAL DEPOSIT:") || k.StartsWith("DEPOSIT CORRECTION:"))
                .ToList();
            foreach (var k in depositKeys) regularSalesSummary.Remove(k);

            // Group regular sales
            var itemList = dbItems?.ToList() ?? new List<LiquorItem>();
            var tokenList = dbTokens?.ToList() ?? new List<PosToken>();

            var groupedRegular = regularSalesSummary
                .Select(kvp => {
                    var cleanKey = kvp.Key;
                    if (cleanKey.EndsWith(" (CREDITED)")) cleanKey = cleanKey.Replace(" (CREDITED)", "");
                    bool isTokenSale = cleanKey.Contains(" (TOKEN SALE)");
                    if (isTokenSale) cleanKey = cleanKey.Replace(" (TOKEN SALE)", "");
                    bool isTokenRedeemed = cleanKey.Contains(" (TOKEN REDEEMED)");
                    if (isTokenRedeemed) cleanKey = cleanKey.Replace(" (TOKEN REDEEMED)", "");

                    var product = itemList.FirstOrDefault(i => i.Name != null && i.Name.Equals(cleanKey, StringComparison.OrdinalIgnoreCase));
                    if (product == null && cleanKey.Contains(" (") && cleanKey.EndsWith(")"))
                    {
                        int openIdx = cleanKey.IndexOf(" (");
                        string baseName = cleanKey.Substring(0, openIdx).Trim();
                        product = itemList.FirstOrDefault(i => i.Name != null && i.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase));
                        if (product == null)
                        {
                            string innerName = cleanKey.Substring(openIdx + 2, cleanKey.Length - openIdx - 3).Trim();
                            product = itemList.FirstOrDefault(i => i.Name != null && i.Name.Equals(innerName, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    var category = product?.Category;
                    if (kvp.Key.Contains("(DARTS")) category = "DARTS ROUND";
                    else if (kvp.Key.StartsWith("TAB DEPOSIT:")) category = "DEPOSITS";
                    else if (category == null && (kvp.Key.Contains("TOKEN CREDIT") || kvp.Key.Contains("(TOKEN REDEEMED)") || kvp.Key.Contains("TOKEN"))) category = "TOKENS";
                    else if (isTokenRedeemed || kvp.Key.Contains("(TOKEN REDEEMED)") || kvp.Key.Contains("TOKEN REDEEMED")) category = "TOKENS";

                    decimal lineTotal = 0;
                    if (itemTotals.TryGetValue(kvp.Key, out var exactTotal))
                    {
                        lineTotal = exactTotal;
                    }
                    else
                    {
                        decimal price = 0;
                        if (isTokenRedeemed || kvp.Key.Contains("(TOKEN REDEEMED)") || kvp.Key.Contains("TOKEN REDEEMED"))
                        {
                            price = 0;
                        }
                        else if (isTokenSale || kvp.Key.Contains("(TOKEN SALE)") || kvp.Key.Contains("TOKEN SALE"))
                        {
                            var tokenMatch = tokenList.OrderByDescending(t => t.Name.Length)
                                .FirstOrDefault(t => cleanKey.StartsWith(t.Name, StringComparison.OrdinalIgnoreCase) || kvp.Key.StartsWith(t.Name, StringComparison.OrdinalIgnoreCase));
                            if (tokenMatch != null) price = tokenMatch.SalePrice;
                            else if (product != null) price = (product.RetailPrice > 0) ? product.RetailPrice : product.CurrentPrice;
                        }
                        else
                        {
                            if (product != null)
                            {
                                price = (product.RetailPrice > 0) ? product.RetailPrice : product.CurrentPrice;
                            }
                            if (price == 0 && (kvp.Key.Contains("TOKEN") || kvp.Key.Contains("Token")))
                            {
                                var tokenMatch = tokenList.OrderByDescending(t => t.Name.Length)
                                    .FirstOrDefault(t => cleanKey.StartsWith(t.Name, StringComparison.OrdinalIgnoreCase) || kvp.Key.StartsWith(t.Name, StringComparison.OrdinalIgnoreCase));
                                if (tokenMatch != null) price = tokenMatch.SalePrice;
                            }
                        }
                        
                        if (kvp.Key.StartsWith("> ") && kvp.Key.Contains(" TOKEN CREDIT FOR "))
                        {
                            var tokenPart = kvp.Key.Substring(2, kvp.Key.IndexOf(" TOKEN CREDIT FOR ") - 2).Trim();
                            var token = tokenList.FirstOrDefault(t => t.Name.Equals(tokenPart, StringComparison.OrdinalIgnoreCase));
                            if (token != null) price = -(token.CreditValue ?? token.SalePrice);
                        }
                        lineTotal = price * kvp.Value;
                    }
                    return new { Name = kvp.Key, Quantity = kvp.Value, Category = category ?? "MISC", Total = lineTotal, ZReportGroup = product?.ZReportGroup ?? 0 };
                })
                .GroupBy(x => x.Category)
                .OrderBy(g => g.Key)
                .ToList();

            // Financial Summary Section
            AppendSectionHeader(sb, "FINANCIAL SUMMARY");
            AppendBlank(sb);
            AppendLine(sb, "GROSS SALES", $"{totalGrossSales:C}");
            if (totalPrepaidDeposits > 0)
                AppendLine(sb, "PREPAID FUNDS DEPOSITED", $"+{totalPrepaidDeposits:C}");
            if (totalPrepaidReturned < 0)
                AppendLine(sb, "PREPAID FUNDS RETURNED", $"-{Math.Abs(totalPrepaidReturned):C}");
            if (tokenCredits > 0)
                AppendLine(sb, "TOKEN CREDITS", $"-{tokenCredits:C}");
            if (payoutTotal > 0)
                AppendLine(sb, "TOTAL PAYOUTS", $"-{payoutTotal:C}");
            if (noSaleCount > 0)
                AppendLine(sb, "NO SALE COUNT", noSaleCount.ToString());
            AppendBlank(sb);

            if (payoutTotal > 0)
            {
                AppendLine(sb, "CASH SALES", $"{cashTotal:C}");
                AppendLine(sb, "TOTAL PAYOUTS", $"-{payoutTotal:C}");
                AppendLine(sb, "EXPECTED CASH IN DRAWER", $"{(cashTotal - payoutTotal):C}");
                AppendBlank(sb);
                foreach (var wl in WrapText("* Expected cash in drawer = Cash Sales minus Payouts.", 0))
                    sb.Append(wl).Append('\n');
            }
            else
            {
                AppendLine(sb, "CASH IN DRAWER", $"{cashTotal:C}");
            }

            // Category Summaries
            decimal foodSnackTotal = 0;
            decimal liquorTotal = 0;
            foreach (var group in groupedRegular)
            {
                foreach (var item in group)
                {
                    var itemCat = item.Category.ToUpper();
                    var itemNameLower = item.Name.ToLower();

                    int effectiveGroup = item.ZReportGroup;

                    if (effectiveGroup == 1) foodSnackTotal += item.Total;
                    else if (effectiveGroup == 2) liquorTotal += item.Total;
                    else if (effectiveGroup == 3) { /* Excluded */ }
                    else
                    {
                        if (itemCat == "FOOD" || itemCat == "CANDY" || itemCat == "SNACK" || itemCat == "SNACKS" || itemCat == "NON-ALCOHOLIC" || itemCat == "BEVERAGE" || itemCat == "BEVERAGES" || itemCat == "SODA" || itemCat == "WATER" || itemNameLower.Contains("soda") || itemNameLower.Contains("water") || itemNameLower.Contains("redbull") || itemNameLower.Contains("red bull"))
                        {
                            foodSnackTotal += item.Total;
                        }
                        else
                        {
                            liquorTotal += item.Total;
                        }
                    }
                }
            }

            AppendSectionHeader(sb, "CATEGORY SUMMARIES");
            AppendBlank(sb);
            AppendLine(sb, "FOOD & SNACKS TOTAL", $"{foodSnackTotal:C}");
            AppendLine(sb, "LIQUOR & DRINKS TOTAL", $"{liquorTotal:C}");
            AppendBlank(sb);

            // Drawer Payouts
            if (payoutLines.Any())
            {
                AppendSectionHeader(sb, "DRAWER PAYOUTS");
                foreach (var (lbl, amt) in payoutLines)
                    AppendLine(sb, lbl, amt);
                AppendDivider(sb, '-');
                AppendLine(sb, "TOTAL PAYOUTS", $"-{payoutTotal:C}");
            }

            // Shift Breakdown
            AppendSectionHeader(sb, "SHIFT BREAKDOWN");
            foreach (var group in groupedRegular)
            {
                AppendBlank(sb);
                AppendLeft(sb, group.Key.ToUpper());
                foreach (var item in group.OrderByDescending(x => x.Quantity))
                {
                    var qty = $"{item.Quantity} x";
                    var name = item.Name.ToUpper();
                    var combined = qty.PadRight(4) + " " + name;
                    if (combined.Length > Width) combined = combined[..Width];
                    sb.Append(combined.PadRight(Width)).Append('\n');
                }
                var groupTotal = group.Sum(x => x.Total);
                AppendLine(sb, $"  {group.Key} TOTAL", $"{groupTotal:C}");
            }

            // Event Activity
            if (banquetSummary.Any())
            {
                AppendSectionHeader(sb, "EVENT ACTIVITY");
                foreach (var b in banquetSummary)
                {
                    var isRunningTab = b.EventType == "RunningTab";
                    AppendBlank(sb);
                    AppendLeft(sb, $"EVENT: {b.EventName.ToUpper()}");
                    AppendDivider(sb, '.');
                    if (!isRunningTab && b.Deposits.Any(d => d >= 0))
                    {
                        AppendLeft(sb, "DEPOSITS:");
                        for (int i = 0; i < b.Deposits.Count; i++)
                        {
                            var dep = b.Deposits[i];
                            if (dep < 0) continue;
                            var lbl = i == 0 ? "  - Initial Deposit" : "  - Add Funds";
                            var amt = $"{dep:C}";
                            AppendLine(sb, lbl, amt);
                        }
                    }
                    AppendLeft(sb, "TAB PURCHASES:");
                    foreach (var item in b.ItemSummary.OrderByDescending(x => x.Value))
                    {
                        var itemTotal = b.ItemTotals.ContainsKey(item.Key) ? b.ItemTotals[item.Key] : 0;
                        AppendLine(sb, $"  {item.Value} x {item.Key}", $"{itemTotal:C}");
                    }
                    AppendDivider(sb, '.');
                    AppendLine(sb, "TOTAL SPENT", $"{b.TotalSpent:C}");
                    var remaining = b.Deposits.Sum() - b.TotalSpent;
                    if (!isRunningTab)
                        AppendLine(sb, "TAB BALANCE", $"{remaining:C}");
                    else
                        AppendLine(sb, "TOTAL DUE", $"{b.TotalSpent:C}");
                }
            }

            // Inventory Removal Log
            if (inventoryPulls.Any())
            {
                AppendSectionHeader(sb, "INVENTORY REMOVAL LOG");
                foreach (var pull in inventoryPulls)
                {
                    var prod = itemList.FirstOrDefault(i => i.Id == pull.Key);
                    var itemName = prod != null ? prod.Name : $"Item #{pull.Key}";
                    AppendLine(sb, itemName.ToUpper(), pull.Value.ToString());
                }
            }

            // Total Sales footer
            AppendBlank(sb);
            AppendDivider(sb, '=');
            AppendLine(sb, "TOTAL SALES", $"{totalGrossSales:C}");
            AppendDivider(sb, '=');
            AppendBlank(sb);
            AppendCenter(sb, $"REF: {id}");

            return sb.ToString();
        }
    }
}
