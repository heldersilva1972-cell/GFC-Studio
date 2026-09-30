using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.Models
{
    public class RentalPricingGuideModel
    {
        public string HeaderTitle { get; set; } = "EVENT PRICING GUIDE";
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
            var model = new RentalPricingGuideModel
            {
                HeaderTitle = "EVENT PRICING GUIDE",
                HeaderSubtitle = "Private Rentals • Member Pricing • Community Rates • Bar & Add-On Services"
            };
            
            // Resolve tier cards from Tier & Day Matrix Rate Configuration
            var tierCards = settings?.GetTierCardsList() ?? PricingTierDefaults.GetDefaultTierCards();

            var nonMemberCard = tierCards.FirstOrDefault(c => c.AssociatedRenterType == "Non-Member" || c.Title.Contains("Non-Member", StringComparison.OrdinalIgnoreCase)) 
                                ?? (tierCards.Count > 0 ? tierCards[0] : null);
                                
            var memberCard = tierCards.FirstOrDefault(c => c.AssociatedRenterType == "Member" || (c.Title.Contains("Member", StringComparison.OrdinalIgnoreCase) && !c.Title.Contains("Non", StringComparison.OrdinalIgnoreCase))) 
                             ?? (tierCards.Count > 1 ? tierCards[1] : null);
                             
            var nonProfitCard = tierCards.FirstOrDefault(c => c.AssociatedRenterType == "NonProfit" || c.Title.Contains("Non-Profit", StringComparison.OrdinalIgnoreCase) || c.Title.Contains("Charity", StringComparison.OrdinalIgnoreCase)) 
                                ?? (tierCards.Count > 2 ? tierCards[2] : null);

            // Populate Tier Cards (Top 3) matching Matrix Cards day-by-day
            foreach (var card in tierCards.Take(3))
            {
                var guideCard = new PricingGuideTierCard
                {
                    Id = card.Id,
                    Title = card.Title.ToUpperInvariant(),
                    Subtitle = card.Subtitle,
                    FooterNote = card.FooterNote,
                    ThemeColor = GetThemeColorName(card.ThemeColor, card.AssociatedRenterType)
                };

                // Add individual days of the week matching the Matrix setup
                if (card.MondayAvailable || card.MondayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Monday", PriceText = $"${card.MondayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.MondayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Monday", PriceText = $"${card.MondayRate:N0}" });

                if (card.TuesdayAvailable || card.TuesdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Tuesday", PriceText = $"${card.TuesdayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.TuesdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Tuesday", PriceText = $"${card.TuesdayRate:N0}" });

                if (card.WednesdayAvailable || card.WednesdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Wednesday", PriceText = $"${card.WednesdayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.WednesdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Wednesday", PriceText = $"${card.WednesdayRate:N0}" });

                if (card.ThursdayAvailable || card.ThursdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Thursday", PriceText = $"${card.ThursdayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.ThursdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Thursday", PriceText = $"${card.ThursdayRate:N0}" });

                if (card.FridayAvailable || card.FridayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Friday", PriceText = $"${card.FridayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.FridayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Friday", PriceText = $"${card.FridayRate:N0}" });

                if (card.SaturdayAvailable || card.SaturdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Saturday", PriceText = $"${card.SaturdayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.SaturdayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Saturday", PriceText = $"${card.SaturdayRate:N0}" });

                if (card.SundayAvailable || card.SundayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Sunday", PriceText = $"${card.SundayRate:N0}" });
                else if (card.AssociatedRenterType != "NonProfit" || card.SundayRate > 0)
                    guideCard.Items.Add(new PricingGuideItem { Label = "Sunday", PriceText = $"${card.SundayRate:N0}" });

                // Subgroups (e.g., Youth Groups, Local Causes)
                if (card.SubGroups != null && card.SubGroups.Count > 0)
                {
                    foreach (var sg in card.SubGroups.Where(s => s.IsAvailable))
                    {
                        string priceTxt = sg.Rate == 0 ? "$0*" : $"${sg.Rate:N0}";
                        guideCard.Items.Add(new PricingGuideItem { Label = sg.Name, PriceText = priceTxt });
                    }
                }

                model.TierCards.Add(guideCard);
            }

            // Populate Middle Comparison Chart with real day-of-week rates from the 3 Tier Cards
            decimal nmMon = nonMemberCard?.MondayRate ?? 350;
            decimal nmFri = nonMemberCard?.FridayRate ?? 500;
            decimal nmSat = nonMemberCard?.SaturdayRate ?? 650;
            decimal nmSun = nonMemberCard?.SundayRate ?? 450;

            decimal mMon = memberCard?.MondayRate ?? 250;
            decimal mFri = memberCard?.FridayRate ?? 400;
            decimal mSat = memberCard?.SaturdayRate ?? 550;
            decimal mSun = memberCard?.SundayRate ?? 350;

            decimal npMon = nonProfitCard?.MondayRate ?? 100;
            decimal npFri = (nonProfitCard != null && (nonProfitCard.FridayAvailable || nonProfitCard.FridayRate > 0)) ? nonProfitCard.FridayRate : 0;
            decimal npSat = (nonProfitCard != null && (nonProfitCard.SaturdayAvailable || nonProfitCard.SaturdayRate > 0)) ? nonProfitCard.SaturdayRate : 0;
            decimal npSun = (nonProfitCard != null && (nonProfitCard.SundayAvailable || nonProfitCard.SundayRate > 0)) ? nonProfitCard.SundayRate : 0;

            model.ChartRows = new List<PricingComparisonRow>
            {
                new PricingComparisonRow
                {
                    DayLabel = "Mon - Thu",
                    NonMemberAmount = nmMon,
                    MemberAmount = mMon,
                    NonProfitAmount = npMon
                },
                new PricingComparisonRow
                {
                    DayLabel = "Friday",
                    NonMemberAmount = nmFri,
                    MemberAmount = mFri,
                    NonProfitAmount = npFri
                },
                new PricingComparisonRow
                {
                    DayLabel = "Saturday",
                    NonMemberAmount = nmSat,
                    MemberAmount = mSat,
                    NonProfitAmount = npSat
                },
                new PricingComparisonRow
                {
                    DayLabel = "Sunday",
                    NonMemberAmount = nmSun,
                    MemberAmount = mSun,
                    NonProfitAmount = npSun
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

        private static string GetThemeColorName(string? colorHexOrName, string? renterType)
        {
            if (!string.IsNullOrEmpty(colorHexOrName))
            {
                var lower = colorHexOrName.ToLowerInvariant();
                if (lower.Contains("primary") || lower == "#0d6efd" || lower.Contains("0d6efd") || lower.Contains("blue")) return "primary";
                if (lower.Contains("success") || lower == "#198754" || lower.Contains("198754") || lower.Contains("green")) return "success";
                if (lower.Contains("warning") || lower == "#d97706" || lower.Contains("d97706") || lower.Contains("amber") || lower.Contains("gold") || lower.Contains("orange")) return "warning";
            }
            
            if (renterType == "Member") return "success";
            if (renterType == "NonProfit") return "warning";
            return "primary";
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
