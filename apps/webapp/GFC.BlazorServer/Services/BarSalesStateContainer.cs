using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services
{
    public class BarSalesStateContainer
    {
        public List<BarSaleEntry>? Entries { get; set; }
        public decimal GrandTotalRevenue { get; set; }
        public int GrandTotalItemsSold { get; set; }
        public Dictionary<int, decimal> AnnualRevenue { get; set; } = new();
        public List<int> AvailableYears { get; set; } = new();
        public int TotalEntries { get; set; }
        
        // Cache for individual pages to make pagination instant
        public Dictionary<int, List<BarSaleEntry>> PageCache { get; set; } = new();
        
        // Timestamp to track when we should do a background refresh
        public System.DateTime LastRefresh { get; set; }
        
        public bool HasData => Entries != null;
        
        public void Clear()
        {
            Entries = null;
            GrandTotalRevenue = 0;
            GrandTotalItemsSold = 0;
            AvailableYears.Clear();
            TotalEntries = 0;
            PageCache.Clear();
        }
    }
}


