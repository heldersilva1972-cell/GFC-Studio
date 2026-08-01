using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace AuditFix
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
            using var conn = new SqlConnection(connStr);
            conn.Open();

            // Load master liquor items for price lookups
            var dbItems = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            using (var itemCmd = new SqlCommand("SELECT Name, Price FROM PosItems", conn))
            using (var itemReader = itemCmd.ExecuteReader())
            {
                while (itemReader.Read())
                {
                    string name = itemReader["Name"]?.ToString() ?? "";
                    decimal price = itemReader["Price"] != DBNull.Value ? Convert.ToDecimal(itemReader["Price"]) : 0m;
                    if (!string.IsNullOrEmpty(name) && !dbItems.ContainsKey(name))
                    {
                        dbItems[name] = price;
                    }
                }
            }

            Console.WriteLine($"Loaded {dbItems.Count} items from PosItems.");

            // Load Z-reports
            var query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports ORDER BY Timestamp DESC";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var reports = new List<ZReportInfo>();
            while (reader.Read())
            {
                reports.Add(new ZReportInfo
                {
                    Id = (Guid)reader["Id"],
                    TerminalName = reader["TerminalName"].ToString(),
                    BartenderName = reader["BartenderName"].ToString(),
                    Timestamp = (DateTime)reader["Timestamp"],
                    StoredGross = Convert.ToDecimal(reader["TotalGrossSales"]),
                    StoredCash = Convert.ToDecimal(reader["CashTotal"]),
                    Json = reader["SalesSummaryJson"].ToString()
                });
            }
            reader.Close();

            Console.WriteLine($"Loaded {reports.Count} Z-Reports.");

            foreach (var r in reports)
            {
                if (string.IsNullOrEmpty(r.Json) || r.Json == "[]" || r.Json == "{}") continue;

                decimal calculatedItemSum = 0m;
                try
                {
                    // Dict format: {"ItemName": qty}
                    var dict = JsonSerializer.Deserialize<Dictionary<string, int>>(r.Json);
                    if (dict != null)
                    {
                        foreach (var kvp in dict)
                        {
                            string rawName = kvp.Key;
                            int qty = kvp.Value;

                            if (rawName.EndsWith(" (CREDITED)") || rawName.StartsWith("> ") || rawName.Contains("TOKEN CREDIT") || rawName.Contains("(TOKEN REDEEMED)") || rawName.Contains("TOKEN REDEEMED"))
                                continue;

                            string cleanKey = rawName;
                            if (cleanKey.Contains(" (TOKEN SALE)")) cleanKey = cleanKey.Replace(" (TOKEN SALE)", "");
                            if (cleanKey.Contains(" (TOKEN REDEEMED)")) cleanKey = cleanKey.Replace(" (TOKEN REDEEMED)", "");

                            decimal price = 0m;
                            if (dbItems.TryGetValue(cleanKey, out var p))
                            {
                                price = p;
                            }
                            else if (cleanKey.Contains(" (") && cleanKey.EndsWith(")"))
                            {
                                int openIdx = cleanKey.IndexOf(" (");
                                string innerName = cleanKey.Substring(openIdx + 2, cleanKey.Length - openIdx - 3).Trim();
                                if (dbItems.TryGetValue(innerName, out var innerP)) price = innerP;
                            }

                            calculatedItemSum += (price * qty);
                        }
                    }
                }
                catch { }

                decimal diff = r.StoredGross - calculatedItemSum;
                if (Math.Abs(diff) > 0.05m && r.StoredGross > 0)
                {
                    Console.WriteLine($"ZReport [{r.Id}] {r.Timestamp:g} ({r.BartenderName}): Stored Gross=${r.StoredGross:F2}, Item-Calculated=${calculatedItemSum:F2} | Difference=${diff:F2}");
                }
            }
        }
    }

    class ZReportInfo
    {
        public Guid Id { get; set; }
        public string TerminalName { get; set; }
        public string BartenderName { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal StoredGross { get; set; }
        public decimal StoredCash { get; set; }
        public string Json { get; set; }
    }
}
