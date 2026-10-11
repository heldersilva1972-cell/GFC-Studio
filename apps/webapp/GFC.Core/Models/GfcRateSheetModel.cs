using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class GfcRateSheetModel
    {
        // Header
        public string HeaderTitle { get; set; } = "GLOUCESTER FRATERNITY CLUB";
        public string HeaderSubtitle { get; set; } = "Hall Rental Rates & Standard Time Blocks";

        // Standard Time Blocks Strip
        public string TimeBlockTitle { get; set; } = "STANDARD TIME BLOCKS (APPLIES ALL DAYS)";
        public string TimeBlockDescription { get; set; } = "Rentals are reserved in 6-hour fixed blocks.";
        public string DaytimeBlockTitle { get; set; } = "Daytime Block:";
        public string DaytimeBlockHours { get; set; } = "10:00 AM – 4:00 PM";
        public string EveningBlockTitle { get; set; } = "Evening Block:";
        public string EveningBlockHours { get; set; } = "5:00 PM – 11:00 PM";

        // Columns
        public string Col1Header { get; set; } = "Non-Members";
        public string Col2Header { get; set; } = "Members";
        public string Col3Header { get; set; } = "Non-Profits";

        // 7 Days of Rates
        public List<GfcRateDayRow> Days { get; set; } = new()
        {
            new GfcRateDayRow { DayName = "Monday", Col1Price = "$500", Col2Price = "$400", Col3Price = "$150" },
            new GfcRateDayRow { DayName = "Tuesday", Col1Price = "$500", Col2Price = "$400", Col3Price = "$150" },
            new GfcRateDayRow { DayName = "Wednesday", Col1Price = "$500", Col2Price = "$400", Col3Price = "$150" },
            new GfcRateDayRow { DayName = "Thursday", Col1Price = "$500", Col2Price = "$400", Col3Price = "$150" },
            new GfcRateDayRow { DayName = "Friday", Col1Price = "$600", Col2Price = "$500", Col3Price = "Not Available", Col3IsUnavailable = true },
            new GfcRateDayRow { DayName = "Saturday", Col1Price = "$700", Col2Price = "$600", Col3Price = "Not Available", Col3IsUnavailable = true },
            new GfcRateDayRow { DayName = "Sunday", Col1Price = "$550", Col2Price = "$450", Col3Price = "Not Available", Col3IsUnavailable = true }
        };

        // Bottom Left Option Card (Bar Services)
        public string BarCardTitle { get; set; } = "BAR SERVICE OPTIONS";
        public List<GfcServiceOptionItem> BarServices { get; set; } = new()
        {
            new GfcServiceOptionItem
            {
                Name = "Bar Service Package",
                Price = "$100",
                Description = "Provides a dedicated staffed bar.\nAlcohol served at additional drink cost."
            }
        };

        // Bottom Right Option Card (Additional Services)
        public string AddonCardTitle { get; set; } = "ADDITIONAL SERVICES";
        public List<GfcServiceOptionItem> AdditionalServices { get; set; } = new()
        {
            new GfcServiceOptionItem
            {
                Name = "Day-Before Decorating",
                Price = "$100",
                Description = "Early access day before event (when available)."
            }
        };

        // Legacy properties preserved for backward-compatibility during JSON deserialization
        public string? BarPackageName { get; set; }
        public string? BarPackagePrice { get; set; }
        public string? BarPackageDescription { get; set; }
        public string? AddonPackageName { get; set; }
        public string? AddonPackagePrice { get; set; }
        public string? AddonPackageDescription { get; set; }

        public void Normalize()
        {
            if ((BarServices == null || BarServices.Count == 0) && !string.IsNullOrWhiteSpace(BarPackageName))
            {
                BarServices = new List<GfcServiceOptionItem>
                {
                    new GfcServiceOptionItem
                    {
                        Name = BarPackageName ?? "Bar Service Package",
                        Price = BarPackagePrice ?? "$100",
                        Description = BarPackageDescription ?? "Provides a dedicated staffed bar.\nAlcohol served at additional drink cost."
                    }
                };
            }

            if ((AdditionalServices == null || AdditionalServices.Count == 0) && !string.IsNullOrWhiteSpace(AddonPackageName))
            {
                AdditionalServices = new List<GfcServiceOptionItem>
                {
                    new GfcServiceOptionItem
                    {
                        Name = AddonPackageName ?? "Day-Before Decorating",
                        Price = AddonPackagePrice ?? "$100",
                        Description = AddonPackageDescription ?? "Early access day before event (when available)."
                    }
                };
            }

            BarServices ??= new List<GfcServiceOptionItem>();
            AdditionalServices ??= new List<GfcServiceOptionItem>();
        }

        // Footer Note
        public string FooterDisclaimer { get; set; } = "* Rates are per single 6-hour time block. Rental pricing does not include the bar or bartender. Non-profit pricing is restricted to Monday–Thursday.";

        public static GfcRateSheetModel GetDefault()
        {
            var model = new GfcRateSheetModel();
            model.Normalize();
            return model;
        }
    }

    public class GfcServiceOptionItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class GfcRateDayRow
    {
        public string DayName { get; set; } = string.Empty;
        public string Col1Price { get; set; } = "$0";
        public string Col2Price { get; set; } = "$0";
        public string Col3Price { get; set; } = "$0";
        public bool Col1IsUnavailable { get; set; } = false;
        public bool Col2IsUnavailable { get; set; } = false;
        public bool Col3IsUnavailable { get; set; } = false;
    }
}
