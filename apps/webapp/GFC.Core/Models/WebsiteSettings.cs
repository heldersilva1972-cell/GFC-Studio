// [NEW]
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class WebsiteSettings
    {
        [Key]
        public int Id { get; set; }

        public string ClubPhone { get; set; } = string.Empty;

        public string ClubAddress { get; set; } = string.Empty;

        public bool MasterEmailKillSwitch { get; set; } = false;

        // Legacy rates (kept for backward compatibility)
        public decimal? MemberRate { get; set; }
        public decimal? NonMemberRate { get; set; }
        public decimal? NonProfitRate { get; set; }
        
        // Detailed Hall Rental Pricing Matrix (Legacy defaults)
        public decimal? FunctionHallNonMemberRate { get; set; } = 400;
        public decimal? FunctionHallMemberRate { get; set; } = 300;
        public decimal? CoalitionNonMemberRate { get; set; } = 200;
        public decimal? CoalitionMemberRate { get; set; } = 100;
        public decimal? YouthOrganizationNonMemberRate { get; set; } = 100;
        public decimal? YouthOrganizationMemberRate { get; set; } = 100;
        
        // Add-on Services (Legacy defaults)
        public decimal? BartenderServiceFee { get; set; } = 100;
        public decimal? KitchenFee { get; set; } = 50;
        public decimal? AvEquipmentFee { get; set; } = 25;
        public decimal? SecurityDepositAmount { get; set; } = 200;
        public bool RequireSecurityDeposit { get; set; } = true;

        // Dynamic Rooms & Add-on Services JSON Storage
        public string? RoomsJson { get; set; }
        public string? AddonsJson { get; set; }

        // Form Customization & Content
        public string RentalFormTitle { get; set; } = "Hall Rental Application";
        public string RentalFormSubtitle { get; set; } = "Gloucester Fraternity Club";
        public string RentalFormIntroText { get; set; } = "27 Webster Street, Gloucester, MA 01930 | (978) 283-2889";
        public string RentalFormRulesText { get; set; } = "I have read and agree to all GFC rental policies. I understand that date confirmation is subject to committee approval and receipt of the security deposit.";
        public string RentalFormSuccessMessage { get; set; } = "Your rental request has been received and added to our calendar as Pending. The Hall Rental Committee will review your date and reach out to you directly to confirm booking.";
        public bool ShowAddressField { get; set; } = true;
        public bool ShowGuestCountField { get; set; } = true;

        // Dedicated Policies & Agreements
        public string RentalTermsAndConditionsText { get; set; } = @"The person executing this agreement expressly represents that he or she is TWENTY-ONE (21) years of age or older.

The patron of this agreement must obtain prior approval from the Gloucester Fraternity Club (GFC) for all activities which are planned for the affair. The premises may only be used for those approved activities. Patron agrees to assist the GFC in prohibiting violations and enforcing the provisions of this agreement.

The patron of this agreement is responsible to protect all group members from alcohol abuses and holds the GFC harmless.

The patron of this agreement agrees that the GFC will be left in the same condition as was found.

The patron will be responsible for any damage to the GFC building, equipment, decorations or fixtures, lost or damaged, during this affair, due to activities of their guests. The GFC will be held harmless by the patron for any loss of or damage to ANY equipment, decorations or fixtures of any third party.

The patron of this agreement agrees NO SCOTCH TAPE or TACKS will be used on equipment, walls or ceilings of the GFC without the HALL RENTAL COMMITTEE’s approval. The patron also agrees not to plug any electrical equipment or run extension cords without first consulting the GFC.

Due to electronic amplification capabilities of some bands equipment, occasionally it is necessary to require the band to stay within acceptable volume limits. This also includes Disc Jockey amplification equipment.

No affair will be permitted to run over the time specified without prior approval.

The patron of this agreement agrees that he or she understands the Gloucester Fraternity Clubs KITCHEN POLICY, ALCOHOL POLICY and CANCELLATION POLICY as stated in this application.

The patron of this rental, agrees flammable substances are not permitted in the building.

When renting the Gloucester Fraternity Club, use of our parking lot is available for your guests, but only the open and available parking spaces. The Gloucester Fraternity Club makes no guarantee of the number of parking spaces available for your function. Our parking lot is open at all times for our members use. Your guests are welcome to park in the open spaces available.";

        public string RentalCancellationPolicyText { get; set; } = "Cancellations must be made at least 30 days prior to the scheduled function date. Cancellations made within 30 days of the event may result in forfeiture of the hall rental fee.";

        public string RentalKitchenPolicyText { get; set; } = "The kitchen may not be used for cooking meals. Caterers may use the kitchen for cooking but only with proof of LIABILITY INSURANCE. The lower ovens on the gas stove are NOT to be used under any circumstances. The pizza ovens may be used for warming food. The refrigerator may be used for storage but please take all perishable items with you. The Dumpster is available for your use. The Gloucester Fraternity Club provides no utensils, pots or pans for use. Arrangements can be made for the use of our coffee pot.";

        public string RentalPolicyUrl { get; set; } = "https://gloucesterfraternityclub.com/hall-rentals/hall-rental-policy/";

        public string RentalPaymentWindowNotice { get; set; } = "After electronically signing this application, and once your date is approved by the Gloucester Fraternity Club (GFC), you will have two (2) business days to complete payment to reserve your date.";

        // Configurable Required Fields for Submission
        public bool RequireApplicantName { get; set; } = true;
        public bool RequireEmail { get; set; } = true;
        public bool RequirePhone { get; set; } = true;
        public bool RequireAddress { get; set; } = false;
        public bool RequireEventType { get; set; } = true;
        public bool RequireGuestCount { get; set; } = false;

        public int? BaseFunctionHours { get; set; } = 5;
        public bool AllowAdditionalHours { get; set; } = true;
        public decimal? AdditionalHourRate { get; set; } = 50;

        public int? MaxHallRentalDurationHours { get; set; } = 8;

        public bool EnableOnlineRentalsPayment { get; set; } = false;
        public string? PaymentGatewayUrl { get; set; }
        public string? PaymentGatewayApiKey { get; set; }
        
        // System Settings
        public string PrimaryColor { get; set; } = "#0D1B2A"; // Midnight Blue
        public string SecondaryColor { get; set; } = "#FFD700"; // Gold
        public string HeadingFont { get; set; } = "Outfit";
        public string BodyFont { get; set; } = "Inter";
        public bool HighAccessibilityMode { get; set; } = false;

        public bool IsClubOpen { get; set; } = true;

        // SEO Settings
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
        public string SeoKeywords { get; set; } = string.Empty;

        public string FormIngestionMode { get; set; } = "NativeForm"; // "NativeForm", "GoogleWebhook", "Simulation"
        public string WebhookApiKey { get; set; } = Guid.NewGuid().ToString("N");
        public string NotificationEmailList { get; set; } = "gfc@gloucesterfraternityclub.com";
        public bool NotifyOnNewSubmission { get; set; } = true;
        public bool NotifyOnPaymentRecorded { get; set; } = true;
        public bool SendApplicantConfirmation { get; set; } = true;
        public bool NotifyOnStatusChange { get; set; } = true;

        // Dedicated Hall Rental Email Logic & Automation Engine
        public string RentalEmailProvider { get; set; } = "SMTP"; // "SMTP" or "Resend"
        public string? RentalSmtpHost { get; set; } = "mail.gloucesterfraternityclub.com";
        public int RentalSmtpPort { get; set; } = 587;
        public string? RentalSmtpUsername { get; set; }
        public string? RentalSmtpPassword { get; set; }
        public bool RentalSmtpEnableSsl { get; set; } = true;
        public string? RentalResendApiKey { get; set; }
        public string RentalSenderEmail { get; set; } = "rentals@gloucesterfraternityclub.com";
        public string RentalSenderName { get; set; } = "Gloucester Fraternity Club - Hall Rentals";
        public string? RentalEmailCc { get; set; }

        // Customizable Outgoing Response / Auto-Responder Email
        public string ApplicantConfirmationEmailSubject { get; set; } = "Your Hall Rental Application Confirmation - {ClubName}";
        public string ApplicantConfirmationEmailBody { get; set; } = @"Hello {ApplicantName},

Thank you for submitting your Hall Rental Application for {ClubName}.

Event Details:
- Date: {EventDate}
- Room / Space: {RoomSelected}
- Estimated Total: ${TotalPrice}
{DepositDetails}

Our Rental Committee has received your request and added it as Pending to our calendar. We will review your application and contact you shortly.

If you have any questions, please contact us at {ClubPhone}.

Warm regards,
{ClubName} Hall Rental Committee";

        // Day-of-Week Rental Schedules & Time Slot Periods (JSON)
        public string? DaySchedulesJson { get; set; }

        // Sandbox & Testing Configuration
        public bool EnableSandboxMode { get; set; } = false;
        public string SandboxCalendarName { get; set; } = "GFC Test Rental Calendar";
        public string? SandboxTestEmail { get; set; }
        public string? SandboxGoogleCalendarId { get; set; }
        public string? SandboxCalendarFeedUrl { get; set; }

        // Helper methods for dynamic recipient boxes
        public List<string> GetRecipientsList()
        {
            if (string.IsNullOrWhiteSpace(NotificationEmailList))
            {
                return new List<string> { "gfc@gloucesterfraternityclub.com" };
            }

            var items = NotificationEmailList
                .Split(new[] { ',', ';', '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct(System.StringComparer.OrdinalIgnoreCase)
                .ToList();

            return items.Any() ? items : new List<string> { "gfc@gloucesterfraternityclub.com" };
        }

        public void SetRecipientsList(IEnumerable<string> list)
        {
            if (list == null)
            {
                NotificationEmailList = string.Empty;
                return;
            }

            var clean = list
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e.Trim())
                .Distinct(System.StringComparer.OrdinalIgnoreCase);

            NotificationEmailList = string.Join(", ", clean);
        }

        // Helper methods for day-of-week schedules and multi-slot periods
        public List<DayOfWeekScheduleConfig> GetDaySchedulesList()
        {
            if (!string.IsNullOrWhiteSpace(DaySchedulesJson))
            {
                try
                {
                    var items = JsonSerializer.Deserialize<List<DayOfWeekScheduleConfig>>(DaySchedulesJson);
                    if (items != null && items.Count == 7) return items;
                }
                catch { }
            }

            // Default 7 days of the week configuration
            return GetDefaultDaySchedules();
        }

        public static List<DayOfWeekScheduleConfig> GetDefaultDaySchedules()
        {
            var list = new List<DayOfWeekScheduleConfig>();
            var days = new[]
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            foreach (var d in days)
            {
                var isWeekend = d == DayOfWeek.Friday || d == DayOfWeek.Saturday || d == DayOfWeek.Sunday;
                var config = new DayOfWeekScheduleConfig
                {
                    Day = d,
                    DayName = d.ToString(),
                    IsAvailableForRentals = true,
                    UseCustomDayPricing = isWeekend,
                    MemberRate = isWeekend ? 350 : 250,
                    NonMemberRate = isWeekend ? 450 : 350,
                    UseFixedTimeSlots = false,
                    TimeSlots = new List<DayScheduleSlotConfig>
                    {
                        new() { Name = "Afternoon Block", StartTime = "12:00 PM", EndTime = "5:00 PM", IsActive = true },
                        new() { Name = "Evening Block", StartTime = "6:00 PM", EndTime = "11:00 PM", IsActive = true }
                    }
                };
                list.Add(config);
            }

            return list;
        }

        // Helper methods for dynamic rooms & addons
        public List<RentalRoomOption> GetRoomsList()
        {
            if (!string.IsNullOrWhiteSpace(RoomsJson))
            {
                try
                {
                    var items = JsonSerializer.Deserialize<List<RentalRoomOption>>(RoomsJson);
                    if (items != null && items.Count > 0) return items;
                }
                catch { }
            }

            // Fallback initialized with current rates
            var defaults = RentalPricingDefaults.GetDefaultRooms();
            if (defaults.Count > 0)
            {
                defaults[0].NonMemberRate = FunctionHallNonMemberRate ?? 400;
                defaults[0].MemberRate = FunctionHallMemberRate ?? 300;
            }
            if (defaults.Count > 1)
            {
                defaults[1].NonMemberRate = CoalitionNonMemberRate ?? 200;
                defaults[1].MemberRate = CoalitionMemberRate ?? 100;
            }
            if (defaults.Count > 2)
            {
                defaults[2].NonMemberRate = YouthOrganizationNonMemberRate ?? 100;
                defaults[2].MemberRate = YouthOrganizationMemberRate ?? 100;
            }
            return defaults;
        }

        public List<RentalAddonOption> GetAddonsList()
        {
            if (!string.IsNullOrWhiteSpace(AddonsJson))
            {
                try
                {
                    var items = JsonSerializer.Deserialize<List<RentalAddonOption>>(AddonsJson);
                    if (items != null && items.Count > 0) return items;
                }
                catch { }
            }

            var defaults = RentalPricingDefaults.GetDefaultAddons();
            if (defaults.Count > 0) defaults[0].Fee = BartenderServiceFee ?? 100;
            if (defaults.Count > 1) defaults[1].Fee = KitchenFee ?? 50;
            if (defaults.Count > 2) defaults[2].Fee = AvEquipmentFee ?? 25;
            return defaults;
        }

        public string? PoliciesJson { get; set; }

        public List<RentalPolicyItem> GetPoliciesList()
        {
            if (!string.IsNullOrWhiteSpace(PoliciesJson))
            {
                try
                {
                    var items = JsonSerializer.Deserialize<List<RentalPolicyItem>>(PoliciesJson);
                    if (items != null && items.Count > 0) return items;
                }
                catch { }
            }

            var defaults = RentalPricingDefaults.GetDefaultPolicies();
            if (!string.IsNullOrWhiteSpace(RentalTermsAndConditionsText) && defaults.Count > 0)
            {
                defaults[0].Content = RentalTermsAndConditionsText;
            }
            if (!string.IsNullOrWhiteSpace(RentalCancellationPolicyText) && defaults.Count > 1)
            {
                defaults[1].Content = RentalCancellationPolicyText;
            }
            if (!string.IsNullOrWhiteSpace(RentalKitchenPolicyText) && defaults.Count > 3)
            {
                defaults[3].Content = RentalKitchenPolicyText;
            }
            return defaults;
        }
    }

    public class DayOfWeekScheduleConfig
    {
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool UseCustomDayPricing { get; set; } = false;
        public bool UseFixedTimeSlots { get; set; } = false;
        public decimal? CustomMemberRate { get; set; }
        public decimal? CustomNonMemberRate { get; set; }
        public string? AllowedRoomName { get; set; }
        public List<string> AllowedRoomNames { get; set; } = new();
        public List<DayScheduleSlotConfig> Slots { get; set; } = new();

        // Compatibility aliases for JSON serialization and alternative naming
        [System.Text.Json.Serialization.JsonIgnore]
        public string? AllowedRoom { get => AllowedRoomName; set => AllowedRoomName = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DayOfWeek Day { get => DayOfWeek; set => DayOfWeek = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string DayName { get => DayOfWeek.ToString(); set { } }
        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsAvailableForRentals { get => IsAvailable; set => IsAvailable = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public decimal? MemberRate { get => CustomMemberRate; set => CustomMemberRate = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public decimal? NonMemberRate { get => CustomNonMemberRate; set => CustomNonMemberRate = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public List<DayScheduleSlotConfig> TimeSlots { get => Slots; set => Slots = value; }
    }

    public class DayScheduleSlotConfig
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Label { get; set; } = "Time Slot";
        public string StartTime { get; set; } = "8:00 AM";
        public string EndTime { get; set; } = "12:00 PM";
        public decimal? CustomMemberRate { get; set; }
        public decimal? CustomNonMemberRate { get; set; }
        public bool IsActive { get; set; } = true;

        [System.Text.Json.Serialization.JsonIgnore]
        public string Name { get => Label; set => Label = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public decimal? MemberRate { get => CustomMemberRate; set => CustomMemberRate = value; }
        [System.Text.Json.Serialization.JsonIgnore]
        public decimal? NonMemberRate { get => CustomNonMemberRate; set => CustomNonMemberRate = value; }
    }
}
