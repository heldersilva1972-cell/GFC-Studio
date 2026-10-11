using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class GfcRentalPolicyGuideModel
    {
        // Header & Intro
        public string HeaderTitle { get; set; } = "HALL RENTAL AGREEMENT & POLICY GUIDE";
        public string HeaderSubtitle { get; set; } = "Gloucester Fraternity Club";
        public string IntroText { get; set; } = "Thank you for choosing the Gloucester Fraternity Club for your event. To reserve the hall, please review the policies below, complete the application section, and return it with your rental fee and refundable security deposit.";

        // Policy Sections
        public List<GfcPolicySection> Sections { get; set; } = new();

        // Footer Contact Card
        public string ContactRole { get; set; } = "Hall Manager";
        public string ContactName { get; set; } = "Michael Stanger";
        public string ContactPhone { get; set; } = "978-283-2889";
        public string ContactEmailUrl { get; set; } = "https://gloucesterfraternityclub.com/discovery/contact-us-2/";
        public string ContactEmailLabel { get; set; } = "Contact us via Email";

        public static GfcRentalPolicyGuideModel GetDefault()
        {
            return new GfcRentalPolicyGuideModel
            {
                Sections = new List<GfcPolicySection>
                {
                    new GfcPolicySection
                    {
                        Number = "1",
                        Title = "RESERVATIONS & PAYMENT",
                        Clauses = new List<GfcPolicyClause>
                        {
                            new GfcPolicyClause
                            {
                                Label = "Tentative Dates",
                                Text = "Date requests are finalized only after this application, the full rental fee, and the security deposit are received and approved by the club."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Member Pricing",
                                Text = "Available to Regular, Guest, or Life members in good standing. The member must fill out the application, pay all fees directly (checks/payments must be in the member’s name), and assume direct responsibility for the event."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Non-Member / Organization Pricing",
                                Text = "Applies to the general public, community organizations, and events booked by members for outside parties."
                            },
                            new GfcPolicyClause
                            {
                                Label = "50th Wedding Anniversary",
                                Text = "Regular, Guest, or Life members celebrating their 50th wedding anniversary receive complimentary hall rental and club-provided bartender coverage. The standard janitorial fee and security deposit still apply."
                            }
                        }
                    },
                    new GfcPolicySection
                    {
                        Number = "2",
                        Title = "SECURITY DEPOSIT & CLEANUP GUIDELINES",
                        Clauses = new List<GfcPolicyClause>
                        {
                            new GfcPolicyClause
                            {
                                Label = "Security Deposit",
                                Text = "A refundable deposit is collected at the time of booking to secure your date."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Trash Removal",
                                Text = "All event rubbish, waste, and food remnants must be completely bagged and taken out to the exterior club dumpster immediately following the event."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Hall & Kitchen Care",
                                Text = "Tables, countertops, prep surfaces, and sinks must be wiped clean. Floors must be swept of all loose debris and clutter."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Deductions",
                                Text = "If the hall or kitchen is left unclean, or if rubbish is not taken to the dumpster, the security deposit will be forfeited or applied directly to club janitorial service fees."
                            }
                        }
                    },
                    new GfcPolicySection
                    {
                        Number = "3",
                        Title = "KITCHEN POLICIES & CATERING",
                        Clauses = new List<GfcPolicyClause>
                        {
                            new GfcPolicyClause
                            {
                                Label = "Utensils & Cookware",
                                Text = "The club does not supply utensils, pots, pans, serveware, dishware, or linens. Renters and caterers must bring all needed food service items."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Light Kitchen Use (Included)",
                                Text = "Standard rentals include access for warming precooked items (e.g., meatballs, appetizers) or setting out cold trays/desserts."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Full On-Site Cooking ($100 Fee)",
                                Text = "Preparing meals from scratch on-site requires advance approval and an extra $100 kitchen fee. (Waived for qualifying club members)."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Commercial Caterers",
                                Text = "Any hired commercial caterer must provide a copy of their commercial Certificate of Insurance (COI) prior to using the facility. Caterers cooking full meals on-site are subject to the $100 kitchen fee."
                            }
                        }
                    },
                    new GfcPolicySection
                    {
                        Number = "4",
                        Title = "GENERAL VENUE RULES & SECURITY",
                        Clauses = new List<GfcPolicyClause>
                        {
                            new GfcPolicyClause
                            {
                                Label = "Capacity",
                                Text = "Guest counts must stay within approved limits and local fire codes."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Security / Police Detail",
                                Text = "The club reserves the right to require an official police detail at the renter’s expense depending on event size, crowd, or function type."
                            },
                            new GfcPolicyClause
                            {
                                Label = "Advertising",
                                Text = "If the event is hosted by a group or organization, no external advertising, social media flyers, or public ticket sales may occur without advance club approval."
                            }
                        }
                    }
                }
            };
        }
    }

    public class GfcPolicySection
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Number { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<GfcPolicyClause> Clauses { get; set; } = new();
    }

    public class GfcPolicyClause
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Label { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
