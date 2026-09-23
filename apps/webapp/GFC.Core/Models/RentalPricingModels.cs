using System.Collections.Generic;
using System.Text.Json;

namespace GFC.Core.Models
{
    public class RentalRoomOption
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal NonMemberRate { get; set; } = 400;
        public decimal MemberRate { get; set; } = 300;
        public int Capacity { get; set; } = 180;
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    public class RentalAddonOption
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fee { get; set; } = 50;
        public bool IsActive { get; set; } = true;
    }

    public class RentalPolicyItem
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString("N");
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CheckboxLabel { get; set; } = "I have read, understand, and agree to the above *";
        public string DisplayMode { get; set; } = "Always"; // "Always", "AddonTriggered", "QuestionTriggered", "Hidden"
        public string? AssociatedAddonId { get; set; } // e.g., "addon_bar", "addon_kitchen"
        public string? PromptQuestion { get; set; } // Question asked to applicant (e.g. "Do you require setup time prior to your event start time listed above?")
        public bool IsRequired { get; set; } = true;
    }

    public static class RentalPricingDefaults
    {
        public static List<RentalRoomOption> GetDefaultRooms()
        {
            return new List<RentalRoomOption>
            {
                new RentalRoomOption
                {
                    Id = "room_fh",
                    Name = "Function Hall",
                    Description = "Main banquet hall with bar & dance floor",
                    NonMemberRate = 400,
                    MemberRate = 300,
                    Capacity = 180,
                    IsDefault = true,
                    IsActive = true
                },
                new RentalRoomOption
                {
                    Id = "room_cr",
                    Name = "Coalition Room",
                    Description = "Meeting & small event space",
                    NonMemberRate = 200,
                    MemberRate = 100,
                    Capacity = 40,
                    IsActive = true
                },
                new RentalRoomOption
                {
                    Id = "room_yo",
                    Name = "Youth Organizations",
                    Description = "Community non-profit rate",
                    NonMemberRate = 100,
                    MemberRate = 100,
                    Capacity = 100,
                    IsActive = true
                }
            };
        }

        public static List<RentalAddonOption> GetDefaultAddons()
        {
            return new List<RentalAddonOption>
            {
                new RentalAddonOption
                {
                    Id = "addon_bar",
                    Name = "Bar Service (alcohol & bartender – additional cost)",
                    Description = "Selecting this option provides a staffed bar with alcoholic beverages. If not selected, guests are strictly prohibited from bringing alcohol onto the premises under any circumstances.",
                    Fee = 100,
                    IsActive = true
                },
                new RentalAddonOption
                {
                    Id = "addon_kitchen",
                    Name = "Kitchen Access",
                    Description = "Food prep & warming kitchen usage",
                    Fee = 50,
                    IsActive = true
                },
                new RentalAddonOption
                {
                    Id = "addon_av",
                    Name = "Special AV Equipment",
                    Description = "Projector, sound system & microphone",
                    Fee = 25,
                    IsActive = true
                }
            };
        }

        public static List<RentalPolicyItem> GetDefaultPolicies()
        {
            return new List<RentalPolicyItem>
            {
                new RentalPolicyItem
                {
                    Id = "policy_terms",
                    Title = "GLOUCESTER FRATERNITY CLUB FUNCTION HALL RENTAL TERMS AND CONDITIONS AGREEMENT",
                    Content = @"The person executing this agreement expressly represents that he or she is TWENTY-ONE (21) years of age or older.

The patron of this agreement must obtain prior approval from the Gloucester Fraternity Club (GFC) for all activities which are planned for the affair. The premises may only be used for those approved activities. Patron agrees to assist the GFC in prohibiting violations and enforcing the provisions of this agreement.

The patron of this agreement is responsible to protect all group members from alcohol abuses and holds the GFC harmless.

The patron of this agreement agrees that the GFC will be left in the same condition as was found.

The patron will be responsible for any damage to the GFC building, equipment, decorations or fixtures, lost or damaged, during this affair, due to activities of their guests. The GFC will be held harmless by the patron for any loss of or damage to ANY equipment, decorations or fixtures of any third party.

The patron of this agreement agrees NO SCOTCH TAPE or TACKS will be used on equipment, walls or ceilings of the GFC without the HALL RENTAL COMMITTEE’s approval. The patron also agrees not to plug any electrical equipment or run extension cords without first consulting the GFC.

Due to electronic amplification capabilities of some bands equipment, occasionally it is necessary to require the band to stay within acceptable volume limits. This also includes Disc Jockey amplification equipment.

No affair will be permitted to run over the time specified without prior approval.

The patron of this agreement agrees that he or she understands the Gloucester Fraternity Clubs KITCHEN POLICY, ALCOHOL POLICY and CANCELLATION POLICY as stated in this application.

The patron of this rental, agrees flammable substances are not permitted in the building.

When renting the Gloucester Fraternity Club, use of our parking lot is available for your guests, but only the open and available parking spaces. The Gloucester Fraternity Club makes no guarantee of the number of parking spaces available for your function. Our parking lot is open at all times for our members use. Your guests are welcome to park in the open spaces available.",
                    CheckboxLabel = "I Agree to the Terms and Conditions above *",
                    DisplayMode = "Always",
                    AssociatedAddonId = null,
                    IsRequired = true
                },
                new RentalPolicyItem
                {
                    Id = "policy_cancel",
                    Title = "Cancellation Policy",
                    Content = "Cancellations must be made at least 30 days prior to the scheduled function date. Cancellations made within 30 days of the event may result in forfeiture of the hall rental fee.",
                    CheckboxLabel = "I have read and understand the cancellation policy above *",
                    DisplayMode = "Always",
                    AssociatedAddonId = null,
                    IsRequired = true
                },
                new RentalPolicyItem
                {
                    Id = "policy_setup",
                    Title = "Event Setup & Takedown Policy",
                    PromptQuestion = "Do you require setup time prior to your event start time listed above?",
                    Content = "Please note: The hall will be empty upon your arrival. Tables and chairs are available for your use, but setup and takedown are the responsibility of the renter. The hall must be returned to its original clean and empty condition at the end of your rental. Extra setup time is subject to availability; we will contact you to review details and confirm what's possible.",
                    CheckboxLabel = "I have read, understand, and agree to the setup and takedown policy above *",
                    DisplayMode = "QuestionTriggered",
                    AssociatedAddonId = null,
                    IsRequired = true
                },
                new RentalPolicyItem
                {
                    Id = "policy_bar",
                    Title = "Bar Service & Alcohol Policy",
                    Content = "Selecting this option provides a staffed bar with alcoholic beverages. If not selected, guests are strictly prohibited from bringing alcohol onto the premises under any circumstances. All guests consuming alcohol must be 21 years of age or older with valid ID.",
                    CheckboxLabel = "I have read, understand, and agree to the Bar Service & Alcohol Policy *",
                    DisplayMode = "AddonTriggered",
                    AssociatedAddonId = "addon_bar",
                    IsRequired = true
                },
                new RentalPolicyItem
                {
                    Id = "policy_kitchen",
                    Title = "GLOUCESTER FRATERNITY CLUB KITCHEN POLICY",
                    Content = "The kitchen may not be used for cooking meals. Caterers may use the kitchen for cooking but only with proof of LIABILITY INSURANCE. The lower ovens on the gas stove are NOT to be used under any circumstances. The pizza ovens may be used for warming food. The refrigerator may be used for storage but please take all perishable items with you. The Dumpster is available for your use. The Gloucester Fraternity Club provides no utensils, pots or pans for use. Arrangements can be made for the use of our coffee pot.",
                    CheckboxLabel = "I have read and understand the Gloucester Fraternity Club Kitchen Policy *",
                    DisplayMode = "AddonTriggered",
                    AssociatedAddonId = "addon_kitchen",
                    IsRequired = true
                }
            };
        }
    }
}
