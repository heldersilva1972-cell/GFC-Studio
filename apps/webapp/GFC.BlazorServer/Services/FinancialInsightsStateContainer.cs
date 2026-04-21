using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services
{
    public class FinancialInsightsStateContainer
    {
        public FinancialSummary? Summary { get; set; }
        public List<int>? AvailableYears { get; set; }
        public List<FinancialDataPoint>? AggregatedData { get; set; }
        
        public bool HasData => Summary != null && AvailableYears != null && AggregatedData != null;
        
        public void Clear()
        {
            Summary = null;
            AvailableYears = null;
            AggregatedData = null;
        }
    }
}


