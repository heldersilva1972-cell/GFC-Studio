using GFC.Core.Models;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services
{
    public class BarSalesStateContainer
    {
        public List<BarSaleEntry>? Entries { get; set; }
        public decimal GrandTotalRevenue { get; set; }
        public int GrandTotalItemsSold { get; set; }
        public List<int> AvailableYears { get; set; } = new();
        public int TotalEntries { get; set; }
        
        public bool HasData => Entries != null;
        
        public void Clear()
        {
            Entries = null;
            GrandTotalRevenue = 0;
            GrandTotalItemsSold = 0;
            AvailableYears.Clear();
            TotalEntries = 0;
        }
    }
}
