using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class PosToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal SalePrice { get; set; }

        public string ColorHex { get; set; } = "#3b82f6"; // Default blue

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Comma-separated list of LiquorItem IDs that this token can cover
        // This is a simplified many-to-many storage for easier migration in this environment
        public string EligibleItemIds { get; set; } = string.Empty;

        // SMART TOKEN UPGRADES
        public bool? AllowCreditUpgrade { get; set; } = false;
        public decimal? CreditValue { get; set; }
        public string? CreditEligibleCategories { get; set; } = string.Empty;
        
        // Helper to ensure we always have a clean bool for UI binding
        public bool IsUpgradeEnabled => AllowCreditUpgrade ?? false;
        
        /// <summary>
        /// Manual adjustment for tokens sold BEFORE the POS system was implemented.
        /// This balance is added to "Sold" count when calculating liability.
        /// </summary>
        public int StartingLiabilityBalance { get; set; } = 0;
    }

}
