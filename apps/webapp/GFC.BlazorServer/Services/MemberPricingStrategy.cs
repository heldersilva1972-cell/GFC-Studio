using System;
using System.Collections.Generic;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    /// <summary>
    /// Strategy class that computes the base rental price from a <see cref="PricingTierCardConfig"/>
    /// based on the rental's member type and event day-of-week.
    /// Replaces the legacy pricing engine; no legacy assumptions are carried forward.
    /// </summary>
    public class MemberPricingStrategy
    {
        private readonly ILogger<MemberPricingStrategy> _logger;

        // Maps member-type keywords (lower-case) to their tier label prefix.
        // Extend this dictionary when new member types are added.
        private static readonly Dictionary<string, string> _tierKeywords = new(StringComparer.OrdinalIgnoreCase)
        {
            { "member",         "Member"        },
            { "new member",     "New Member"    },
            { "social member",  "Social Member" },
            { "non-member",     "Non-Member"    },
            { "non member",     "Non-Member"    },
            { "outside",        "Non-Member"    },
        };

        public MemberPricingStrategy(ILogger<MemberPricingStrategy> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calculates the base rental price for the given member type, event date, and
        /// the list of active pricing-tier cards configured in Hall Rental Settings.
        /// Returns <c>null</c> if no matching tier is found (caller should retain the
        /// existing price or prompt the user).
        /// </summary>
        public decimal? CalculateBasePrice(
            string memberType,
            DateTime eventDate,
            IReadOnlyList<PricingTierCardConfig> tierCards)
        {
            if (string.IsNullOrWhiteSpace(memberType) || tierCards == null || tierCards.Count == 0)
                return null;

            // Resolve the normalised tier label
            var tierLabel = ResolveTierLabel(memberType);
            if (tierLabel == null)
            {
                _logger.LogWarning("MemberPricingStrategy: no tier mapping found for member type '{MemberType}'", memberType);
                return null;
            }

            // Find the first matching card (case-insensitive prefix match)
            PricingTierCardConfig? card = null;
            foreach (var c in tierCards)
            {
                if (!string.IsNullOrWhiteSpace(c.AssociatedRenterType) &&
                    c.AssociatedRenterType.Contains(tierLabel, StringComparison.OrdinalIgnoreCase))
                {
                    card = c;
                    break;
                }
            }

            if (card == null)
            {
                _logger.LogWarning("MemberPricingStrategy: no tier card matched for label '{TierLabel}'", tierLabel);
                return null;
            }

            // Pick the day-of-week rate
            return eventDate.DayOfWeek switch
            {
                DayOfWeek.Friday   => card.FridayAvailable   ? card.FridayRate   : null,
                DayOfWeek.Saturday => card.SaturdayAvailable ? card.SaturdayRate : null,
                DayOfWeek.Sunday   => card.SundayAvailable   ? card.SundayRate   : null,
                _                  => card.MonThuAvailable   ? card.MonThuRate   : null,
            };
        }

        private static string? ResolveTierLabel(string memberType)
        {
            foreach (var kvp in _tierKeywords)
            {
                if (memberType.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }
            return null;
        }
    }
}
