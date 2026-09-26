using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.Models
{
    public class RentalPricingGuideModel
    {
        public string HeaderTitle { get; set; } = "PROPOSED EVENT PRICING GUIDE";
        public string HeaderSubtitle { get; set; } = "Private Rentals • Member Pricing • Community Rates • Bar & Add-On Services";

        // Top 3 Tier Cards (Non-Members, Members, Non-Profits)
        public List<PricingGuideTierCard> TierCards { get; set; } = new();

        // Horizontal Bar Comparison Chart
        public string ChartTitle { get; set; } = "HALL RENTAL RATE COMPARISON (AT-A-GLANCE)";
        public List<PricingComparisonRow> ChartRows { get; set; } = new();

        // Bottom 2 Sections (Bar Services & Additional Services)
        public PricingGuideSection BottomLeftSection { get; set; } = new();
        public PricingGuideSection BottomRightSection { get; set; } = new();

        public static RentalPricingGuideModel CreateDefaultFromSettings(WebsiteSettings? settings)
        {
            var model = new RentalPricingGuideModel();
            
            // Resolve active rooms
            var rooms = settings?.GetRoomsList() ?? RentalPricingDefaults.GetDefaultRooms();
            var stdRoom = rooms.FirstOrDefault(r => r.IsDefault || r.Name.Contains("Standard", StringComparison.OrdinalIgnoreCase) || r.Id == "room_fh") 
                          ?? rooms.FirstOrDefault() 
                          ?? new RentalRoomOption { Name = "Function Hall", NonMemberRate = 400, MemberRate = 300 };
            
            var crRoom = rooms.FirstOrDefault(r => r.Name.Contains("Coalition", StringComparison.OrdinalIgnoreCase) || r.Id == "room_cr")
                         ?? (rooms.Count > 1 ? rooms[1] : new RentalRoomOption { Name = "Coalition Partner Rate", NonMemberRate = 200, MemberRate = 100 });
                         
            var yoRoom = rooms.FirstOrDefault(r => r.Name.Contains("Youth", StringComparison.OrdinalIgnoreCase) || r.Id == "room_yo")
                         ?? (rooms.Count > 2 ? rooms[2] : new RentalRoomOption { Name = "Youth Organization Rate", NonMemberRate = 100, MemberRate = 100 });

            // 1. Non-Members Card
            model.TierCards.Add(new PricingGuideTierCard
            {
                Title = "NON-MEMBERS",
                Subtitle = "Standard Private Event Rates",
                ThemeColor = "primary",
                Items = new List<PricingGuideItem>
                {
                    new PricingGuideItem { Label = "Mon - Thu", PriceText = $"${Math.Max(50, stdRoom.NonMemberRate - 50):N0}" },
                    new PricingGuideItem { Label = "Friday", PriceText = $"${stdRoom.NonMemberRate:N0}" },
                    new PricingGuideItem { Label = "Saturday", PriceText = $"${(stdRoom.NonMemberRate + 150):N0}" },
                    new PricingGuideItem { Label = "Sunday", PriceText = $"${(stdRoom.NonMemberRate + 50):N0}" }
                },
                FooterNote = "Standard Booking T&C"
            });

            // 2. Members Card
            decimal memberSavings = Math.Max(0, stdRoom.NonMemberRate - stdRoom.MemberRate);
            string savingsText = memberSavings > 0 ? $"Member Discount (${memberSavings:N0} Savings)" : "Member Exclusive Rates";
            model.TierCards.Add(new PricingGuideTierCard
            {
                Title = "MEMBERS",
                Subtitle = savingsText,
                ThemeColor = "success",
                Items = new List<PricingGuideItem>
                {
                    new PricingGuideItem { Label = "Mon - Thu", PriceText = $"${Math.Max(50, stdRoom.MemberRate - 50):N0}" },
                    new PricingGuideItem { Label = "Friday", PriceText = $"${stdRoom.MemberRate:N0}" },
                    new PricingGuideItem { Label = "Saturday", PriceText = $"${(stdRoom.MemberRate + 150):N0}" },
                    new PricingGuideItem { Label = "Sunday", PriceText = $"${(stdRoom.MemberRate + 50):N0}" }
                },
                FooterNote = memberSavings > 0 ? $"Save ${memberSavings:N0} across every time slot" : "Standard Member Booking Terms"
            });

            // 3. Non-Profits / Community Card
            model.TierCards.Add(new PricingGuideTierCard
            {
                Title = "NON-PROFITS",
                Subtitle = "Charitable Causes & Community Groups",
                ThemeColor = "warning",
                Items = new List<PricingGuideItem>
                {
                    new PricingGuideItem { Label = "Mon - Thu", PriceText = $"${yoRoom.NonMemberRate:N0}" },
                    new PricingGuideItem { Label = "Fri - Sun", PriceText = $"${crRoom.NonMemberRate:N0}" },
                    new PricingGuideItem { Label = "Youth Groups", PriceText = $"${yoRoom.MemberRate:N0}" },
                    new PricingGuideItem { Label = "Local Causes", PriceText = "$0*" }
                },
                FooterNote = "*Fee waiver option for approved causes"
            });

            // Middle Comparison Chart
            model.ChartRows = new List<PricingComparisonRow>
            {
                new PricingComparisonRow
                {
                    DayLabel = "Mon - Thu",
                    NonMemberAmount = Math.Max(50, stdRoom.NonMemberRate - 50),
                    MemberAmount = Math.Max(50, stdRoom.MemberRate - 50),
                    NonProfitAmount = yoRoom.NonMemberRate
                },
                new PricingComparisonRow
                {
                    DayLabel = "Friday",
                    NonMemberAmount = stdRoom.NonMemberRate,
                    MemberAmount = stdRoom.MemberRate,
                    NonProfitAmount = crRoom.NonMemberRate
                },
                new PricingComparisonRow
                {
                    DayLabel = "Saturday (Peak)",
                    NonMemberAmount = stdRoom.NonMemberRate + 150,
                    MemberAmount = stdRoom.MemberRate + 150,
                    NonProfitAmount = crRoom.NonMemberRate
                },
                new PricingComparisonRow
                {
                    DayLabel = "Sunday",
                    NonMemberAmount = stdRoom.NonMemberRate + 50,
                    MemberAmount = stdRoom.MemberRate + 50,
                    NonProfitAmount = crRoom.NonMemberRate
                }
            };

            // Bottom Left: Bar Service Options
            var addons = settings?.GetAddonsList() ?? RentalPricingDefaults.GetDefaultAddons();
            var barAddon = addons.FirstOrDefault(a => a.Id == "addon_bar" || a.Name.Contains("Bar", StringComparison.OrdinalIgnoreCase));
            decimal barFee = barAddon?.Fee ?? settings?.BartenderServiceFee ?? 100;

            model.BottomLeftSection = new PricingGuideSection
            {
                Title = "BAR SERVICE OPTIONS",
                ThemeColor = "danger",
                Items = new List<PricingGuideServiceItem>
                {
                    new PricingGuideServiceItem
                    {
                        Name = "Bar Service Package",
                        Description = "Dedicated bartender included in package",
                        FeeBadgeText = $"${barFee:N0} flat"
                    },
                    new PricingGuideServiceItem
                    {
                        Name = "Cash Bar Option",
                        Description = "Standard beverage setup & staff service",
                        FeeBadgeText = $"${barFee:N0} setup"
                    },
                    new PricingGuideServiceItem
                    {
                        Name = "Open Bar Option",
                        Description = "Custom beverage package based on guest count",
                        FeeBadgeText = "Custom Quote"
                    }
                }
            };

            // Bottom Right: Additional Services
            decimal overtimeRate = settings?.AdditionalHourRate ?? 75;
            var kitchenAddon = addons.FirstOrDefault(a => a.Id == "addon_kitchen" || a.Name.Contains("Kitchen", StringComparison.OrdinalIgnoreCase));
            decimal kitchenFee = kitchenAddon?.Fee ?? settings?.KitchenFee ?? 50;
            var avAddon = addons.FirstOrDefault(a => a.Id == "addon_av" || a.Name.Contains("AV", StringComparison.OrdinalIgnoreCase));
            decimal avFee = avAddon?.Fee ?? settings?.AvEquipmentFee ?? 25;

            model.BottomRightSection = new PricingGuideSection
            {
                Title = "ADDITIONAL SERVICES",
                ThemeColor = "success",
                Items = new List<PricingGuideServiceItem>
                {
                    new PricingGuideServiceItem
                    {
                        Name = "Extra Event Time",
                        Description = "Additional hours beyond base rental period",
                        FeeBadgeText = $"${overtimeRate:N0} / hr"
                    },
                    new PricingGuideServiceItem
                    {
                        Name = "Kitchen / Food Prep Access",
                        Description = "Commercial warming ovens, prep tables & refrigeration",
                        FeeBadgeText = $"${kitchenFee:N0} flat"
                    },
                    new PricingGuideServiceItem
                    {
                        Name = "Special AV / Decorating Window",
                        Description = "Early access for hall setup & multimedia system usage",
                        FeeBadgeText = $"${Math.Max(avFee, 50):N0} flat"
                    }
                }
            };

            return model;
        }

        public static RentalPricingGuideModel CreateSample2027Model()
        {
            return new RentalPricingGuideModel
            {
                HeaderTitle = "PROPOSED 2027 EVENT PRICING GUIDE",
                HeaderSubtitle = "Private Rentals • Member Pricing • Community Rates • Bar & Add-On Services",
                TierCards = new List<PricingGuideTierCard>
                {
                    new PricingGuideTierCard
                    {
                        Title = "NON-MEMBERS",
                        Subtitle = "Standard Private Event Rates",
                        ThemeColor = "primary",
                        Items = new List<PricingGuideItem>
                        {
                            new PricingGuideItem { Label = "Mon - Thu", PriceText = "$350" },
                            new PricingGuideItem { Label = "Friday", PriceText = "$500" },
                            new PricingGuideItem { Label = "Saturday", PriceText = "$650" },
                            new PricingGuideItem { Label = "Sunday", PriceText = "$450" }
                        },
                        FooterNote = "Standard Booking T&C"
                    },
                    new PricingGuideTierCard
                    {
                        Title = "MEMBERS",
                        Subtitle = "Member Growth ($100 Savings)",
                        ThemeColor = "success",
                        Items = new List<PricingGuideItem>
                        {
                            new PricingGuideItem { Label = "Mon - Thu", PriceText = "$250" },
                            new PricingGuideItem { Label = "Friday", PriceText = "$400" },
                            new PricingGuideItem { Label = "Saturday", PriceText = "$550" },
                            new PricingGuideItem { Label = "Sunday", PriceText = "$350" }
                        },
                        FooterNote = "Save $100 across every time slot"
                    },
                    new PricingGuideTierCard
                    {
                        Title = "NON-PROFITS",
                        Subtitle = "Charitable Causes & Civic Groups",
                        ThemeColor = "warning",
                        Items = new List<PricingGuideItem>
                        {
                            new PricingGuideItem { Label = "Mon - Thu", PriceText = "$100" },
                            new PricingGuideItem { Label = "Fri - Sun", PriceText = "$200" },
                            new PricingGuideItem { Label = "Youth Groups", PriceText = "$100" },
                            new PricingGuideItem { Label = "Local Causes", PriceText = "$0*" }
                        },
                        FooterNote = "*Fee waiver option for approved causes"
                    }
                },
                ChartTitle = "HALL RENTAL RATE COMPARISON (AT-A-GLANCE)",
                ChartRows = new List<PricingComparisonRow>
                {
                    new PricingComparisonRow { DayLabel = "Mon - Thu", NonMemberAmount = 350, MemberAmount = 250, NonProfitAmount = 100 },
                    new PricingComparisonRow { DayLabel = "Friday", NonMemberAmount = 500, MemberAmount = 400, NonProfitAmount = 200 },
                    new PricingComparisonRow { DayLabel = "Saturday (Peak)", NonMemberAmount = 650, MemberAmount = 550, NonProfitAmount = 200 },
                    new PricingComparisonRow { DayLabel = "Sunday", NonMemberAmount = 450, MemberAmount = 350, NonProfitAmount = 200 }
                },
                BottomLeftSection = new PricingGuideSection
                {
                    Title = "BAR SERVICE OPTIONS",
                    ThemeColor = "danger",
                    Items = new List<PricingGuideServiceItem>
                    {
                        new PricingGuideServiceItem { Name = "Bar Service Package", Description = "Dedicated bartender included in package", FeeBadgeText = "$150 flat" },
                        new PricingGuideServiceItem { Name = "Cash Bar Option", Description = "Standard beverage setup & staff", FeeBadgeText = "$150 setup" },
                        new PricingGuideServiceItem { Name = "Open Bar Option", Description = "Custom beverage package based on guest count", FeeBadgeText = "Custom Quote" }
                    }
                },
                BottomRightSection = new PricingGuideSection
                {
                    Title = "ADDITIONAL SERVICES",
                    ThemeColor = "success",
                    Items = new List<PricingGuideServiceItem>
                    {
                        new PricingGuideServiceItem { Name = "Extra Event Time", Description = "Additional hours beyond base rental period", FeeBadgeText = "$75 / hr" },
                        new PricingGuideServiceItem { Name = "Early Setup Access", Description = "Window reserved before standard access time", FeeBadgeText = "$50 flat" },
                        new PricingGuideServiceItem { Name = "Day-Before Decorating", Description = "Venue access day before (when available)", FeeBadgeText = "$100 flat" }
                    }
                }
            };
        }
    }

    public class PricingGuideTierCard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string ThemeColor { get; set; } = "primary"; // primary (blue), success (green), warning (amber)
        public List<PricingGuideItem> Items { get; set; } = new();
        public string FooterNote { get; set; } = string.Empty;
    }

    public class PricingGuideItem
    {
        public string Label { get; set; } = string.Empty;
        public string PriceText { get; set; } = "$0";
    }

    public class PricingComparisonRow
    {
        public string DayLabel { get; set; } = string.Empty;
        public decimal NonMemberAmount { get; set; }
        public decimal MemberAmount { get; set; }
        public decimal NonProfitAmount { get; set; }
    }

    public class PricingGuideSection
    {
        public string Title { get; set; } = string.Empty;
        public string ThemeColor { get; set; } = "danger";
        public List<PricingGuideServiceItem> Items { get; set; } = new();
    }

    public class PricingGuideServiceItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FeeBadgeText { get; set; } = "$0";
    }
}
