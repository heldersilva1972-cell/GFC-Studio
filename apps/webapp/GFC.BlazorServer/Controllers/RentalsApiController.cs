using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/rentals")]
    public class RentalsApiController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly GFC.Core.Interfaces.IWebsiteSettingsService _settingsService;
        private readonly GFC.Core.Interfaces.IMemberRepository _memberRepository;
        private readonly IRentalEmailDispatcher _emailDispatcher;

        public RentalsApiController(
            IRentalService rentalService, 
            GFC.Core.Interfaces.IWebsiteSettingsService settingsService,
            GFC.Core.Interfaces.IMemberRepository memberRepository,
            IRentalEmailDispatcher emailDispatcher)
        {
            _rentalService = rentalService;
            _settingsService = settingsService;
            _memberRepository = memberRepository;
            _emailDispatcher = emailDispatcher;
        }

        public class PossibleMemberDto
        {
            public int MemberId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string MatchReason { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string? Email { get; set; }
        }

        private static string NormalizePersonName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            var s = name.Trim().ToLowerInvariant();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[.,]", " ");
            var parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var suffixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "jr", "sr", "ii", "iii", "iv", "v", "esq", "md", "phd", "dds" };
            parts.RemoveAll(p => suffixes.Contains(p));
            if (parts.Count > 2)
            {
                parts = parts.Where((p, idx) => !(idx > 0 && idx < parts.Count - 1 && p.Length == 1)).ToList();
            }
            return string.Join(" ", parts);
        }

        private static string GetMemberDisplayName(Member? m)
        {
            if (m == null) return string.Empty;
            var name = $"{m.FirstName} {m.LastName}".Trim();
            if (!string.IsNullOrWhiteSpace(m.Suffix))
            {
                name = $"{name}, {m.Suffix.Trim()}";
            }
            return name;
        }

        private static decimal GetCardRateForDate(PricingTierCardConfig card, DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => card.MondayRate,
                DayOfWeek.Tuesday => card.TuesdayRate,
                DayOfWeek.Wednesday => card.WednesdayRate,
                DayOfWeek.Thursday => card.ThursdayRate,
                DayOfWeek.Friday => card.FridayRate,
                DayOfWeek.Saturday => card.SaturdayRate,
                DayOfWeek.Sunday => card.SundayRate,
                _ => card.SaturdayRate
            };
        }

        private static bool IsCardAvailableForDate(PricingTierCardConfig card, DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => card.MondayAvailable,
                DayOfWeek.Tuesday => card.TuesdayAvailable,
                DayOfWeek.Wednesday => card.WednesdayAvailable,
                DayOfWeek.Thursday => card.ThursdayAvailable,
                DayOfWeek.Friday => card.FridayAvailable,
                DayOfWeek.Saturday => card.SaturdayAvailable,
                DayOfWeek.Sunday => card.SundayAvailable,
                _ => false
            };
        }

        private (bool isVerified, int? memberId, string statusText, string badgeType, List<PossibleMemberDto> candidates) VerifyMember(string? name, string? email, string? phone, bool claimedMember)
        {
            var candidates = new List<PossibleMemberDto>();

            // If the applicant is booking under the non-member matrix, do not search or match against the member directory
            if (!claimedMember)
            {
                return (false, null, "Applicant is booking under standard Non-Member pricing.", "NON_MEMBER", candidates);
            }

            if (string.IsNullOrWhiteSpace(name)) 
                return (false, null, "No applicant name provided", "UNVERIFIED_CLAIM", candidates);

            var cleanName = name.Trim().ToLowerInvariant();
            int parenIdx = cleanName.IndexOf('(');
            if (parenIdx > 0) cleanName = cleanName.Substring(0, parenIdx).Trim();
            if (cleanName.StartsWith("hall rental:", StringComparison.OrdinalIgnoreCase)) cleanName = cleanName.Substring(12).Trim();
            if (cleanName.StartsWith("rental:", StringComparison.OrdinalIgnoreCase)) cleanName = cleanName.Substring(7).Trim();

            var cleanPhone = string.IsNullOrEmpty(phone) ? "" : System.Text.RegularExpressions.Regex.Replace(phone, @"[^\d]", "");
            var cleanEmail = (email ?? "").Trim().ToLowerInvariant();

            var normApplicantName = NormalizePersonName(cleanName);

            var nameParts = cleanName.Split(new[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string firstPart = nameParts.Length > 0 ? nameParts[0] : "";
            string lastPart = nameParts.Length > 1 ? nameParts[nameParts.Length - 1] : "";

            // Check if applicant included a suffix in their name (e.g. Jr, Sr, III)
            var commonSuffixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "jr", "sr", "ii", "iii", "iv", "v" };
            string? applicantSuffix = nameParts.FirstOrDefault(p => commonSuffixes.Contains(p.Trim().TrimEnd('.')));

            List<Member> members;
            try
            {
                members = _memberRepository.GetAllMembers() ?? new List<Member>();
            }
            catch
            {
                members = new List<Member>();
            }

            Member? match = null;

            // 1. Match First + Last (+ Suffix if available)
            if (!string.IsNullOrEmpty(firstPart) && !string.IsNullOrEmpty(lastPart))
            {
                match = members.FirstOrDefault(m => 
                    string.Equals((m.FirstName ?? "").Trim(), firstPart, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals((m.LastName ?? "").Trim(), lastPart, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrEmpty(applicantSuffix) || string.Equals((m.Suffix ?? "").Trim().TrimEnd('.'), applicantSuffix, StringComparison.OrdinalIgnoreCase)));

                if (match == null)
                {
                    match = members.FirstOrDefault(m => 
                        string.Equals((m.FirstName ?? "").Trim(), firstPart, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals((m.LastName ?? "").Trim(), lastPart, StringComparison.OrdinalIgnoreCase));
                }
            }

            // 2. Match Full Combined / Normalized Name
            if (match == null && !string.IsNullOrEmpty(cleanName))
            {
                match = members.FirstOrDefault(m => 
                {
                    var mFull = $"{m.FirstName} {m.LastName}".Trim().ToLowerInvariant();
                    var mRev = $"{m.LastName} {m.FirstName}".Trim().ToLowerInvariant();
                    if (mFull == cleanName || mRev == cleanName) return true;

                    var mNorm = NormalizePersonName(mFull);
                    return !string.IsNullOrEmpty(mNorm) && mNorm == normApplicantName;
                });
            }

            // Search for Potential Candidate Matches for Admin Review
            // (Phone, Email, and Last Name matches are presented as candidates for manual review/linking, never auto-verified blindly)
            foreach (var m in members)
            {
                if (match != null && m.MemberID == match.MemberID) continue;

                string mFirst = (m.FirstName ?? "").Trim().ToLowerInvariant();
                string mLast = (m.LastName ?? "").Trim().ToLowerInvariant();
                string mSuffix = (m.Suffix ?? "").Trim().ToLowerInvariant().TrimEnd('.');
                string mPhone = string.IsNullOrEmpty(m.Phone) ? "" : System.Text.RegularExpressions.Regex.Replace(m.Phone, @"[^\d]", "");
                string mEmail = (m.Email ?? "").Trim().ToLowerInvariant();

                string? reason = null;
                if (!string.IsNullOrEmpty(lastPart) && !string.IsNullOrEmpty(mLast) && mLast == lastPart)
                {
                    if (!string.IsNullOrEmpty(applicantSuffix) && !string.IsNullOrEmpty(mSuffix) && applicantSuffix == mSuffix)
                    {
                        reason = $"Same Last Name & Suffix ({m.Suffix?.Trim()})";
                    }
                    else if (!string.IsNullOrEmpty(m.Suffix))
                    {
                        reason = $"Same Last Name (Member is {m.Suffix.Trim()})";
                    }
                    else
                    {
                        reason = "Same Last Name";
                    }
                }
                else if (cleanPhone.Length >= 7 && mPhone.Length >= 7 && (mPhone.EndsWith(cleanPhone) || cleanPhone.EndsWith(mPhone)))
                {
                    reason = "Matching Phone";
                }
                else if (cleanEmail.Length > 5 && mEmail.Length > 5 && mEmail == cleanEmail)
                {
                    reason = "Matching Email";
                }
                else if (!string.IsNullOrEmpty(firstPart) && !string.IsNullOrEmpty(mFirst) && (mFirst == firstPart || mFirst.StartsWith(firstPart)))
                {
                    reason = "Similar First Name";
                }

                if (reason != null && !candidates.Any(c => c.MemberId == m.MemberID))
                {
                    candidates.Add(new PossibleMemberDto
                    {
                        MemberId = m.MemberID,
                        FullName = GetMemberDisplayName(m),
                        Status = string.IsNullOrWhiteSpace(m.Status) ? "Active" : m.Status,
                        MatchReason = reason,
                        Phone = m.Phone,
                        Email = m.Email
                    });

                    if (candidates.Count >= 5) break;
                }
            }

            if (match != null)
            {
                var status = string.IsNullOrWhiteSpace(match.Status) ? "Active" : match.Status;
                var display = GetMemberDisplayName(match);
                return (true, match.MemberID, $"Verified: {display} #{match.MemberID} ({status})", "VERIFIED", candidates);
            }

            var claimText = candidates.Any() 
                ? $"Claimed Member ({candidates.Count} candidate match{(candidates.Count > 1 ? "es" : "")})"
                : "Claimed Member (Not in Directory)";
            return (false, null, claimText, "UNVERIFIED_CLAIM", candidates);
        }

        public class GoogleFormWebhookPayload
        {
            public string? ApiKey { get; set; }
            public string? ApplicantName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public DateTime? EventDate { get; set; }
            public string? EventType { get; set; }
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public string? RoomSelected { get; set; }
            public bool IsClubMember { get; set; }
            public int GuestCount { get; set; }
            public bool BarService { get; set; }
            public bool KitchenAccess { get; set; }
            public bool AvEquipment { get; set; }
            public bool IsTest { get; set; } = false;
        }

        [HttpPost("google-form")]
        public async Task<IActionResult> IngestGoogleForm([FromBody] GoogleFormWebhookPayload payload)
        {
            if (payload == null)
            {
                return BadRequest(new { success = false, message = "Empty payload received." });
            }

            var settings = await _settingsService.GetWebsiteSettingsAsync();

            // Validate API Key if configured
            if (!string.IsNullOrEmpty(settings?.WebhookApiKey) && !string.Equals(settings.WebhookApiKey, payload.ApiKey, StringComparison.Ordinal))
            {
                return Unauthorized(new { success = false, message = "Invalid webhook API key." });
            }

            var request = new HallRentalRequest
            {
                ApplicantName = payload.ApplicantName ?? "Applicant",
                RequesterName = payload.ApplicantName ?? "Applicant",
                RequesterEmail = payload.Email ?? "",
                RequesterPhone = payload.Phone ?? "",
                RequesterAddress = payload.Address,
                EventDate = payload.EventDate ?? DateTime.Today.AddDays(14),
                RequestedDate = payload.EventDate ?? DateTime.Today.AddDays(14),
                EventType = payload.EventType ?? "Hall Rental",
                StartTime = payload.StartTime ?? "2:00 PM",
                EndTime = payload.EndTime ?? "7:00 PM",
                RoomSelected = string.IsNullOrEmpty(payload.RoomSelected) ? "Function Hall" : payload.RoomSelected,
                MemberStatus = payload.IsClubMember,
                RenterType = payload.IsClubMember ? "Member" : "Non-Member",
                GuestCount = payload.GuestCount > 0 ? payload.GuestCount : 50,
                BartenderRequested = payload.BarService,
                KitchenUsage = payload.KitchenAccess,
                AvEquipmentUsage = payload.AvEquipment,
                RulesAgreed = true,
                Status = "Pending",
                IsTestRecord = payload.IsTest
            };

            // Calculate pricing based on settings
            decimal basePrice = payload.IsClubMember ? (settings?.FunctionHallMemberRate ?? 300) : (settings?.FunctionHallNonMemberRate ?? 400);
            decimal addOns = 0;
            var activeAddons = (settings?.GetAddonsList() ?? RentalPricingDefaults.GetDefaultAddons()).Where(a => a.IsActive).ToList();
            var barAddon = activeAddons.FirstOrDefault(a => a.Id == "addon_bar" || a.Name.Contains("Bar", StringComparison.OrdinalIgnoreCase));
            if (payload.BarService && barAddon != null) addOns += barAddon.Fee;
            var kitAddon = activeAddons.FirstOrDefault(a => a.Id == "addon_kitchen" || a.Name.Contains("Kitchen", StringComparison.OrdinalIgnoreCase));
            if (payload.KitchenAccess && kitAddon != null) addOns += kitAddon.Fee;
            var avAddon = activeAddons.FirstOrDefault(a => a.Id == "addon_av" || a.Name.Contains("AV", StringComparison.OrdinalIgnoreCase) || a.Name.Contains("Sound", StringComparison.OrdinalIgnoreCase));
            if (payload.AvEquipment && avAddon != null) addOns += avAddon.Fee;

            request.TotalPrice = basePrice + addOns;
            request.SecurityDepositAmount = settings?.SecurityDepositAmount ?? 200;

            var created = await _rentalService.SubmitPublicRentalRequestAsync(request, request.IsTestRecord);

            return Ok(new
            {
                success = true,
                requestId = created.Id,
                status = created.Status,
                message = "Hall rental application ingested successfully."
            });
        }

        [HttpGet("mobile/list")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetMobileRentalList()
        {
            try
            {
                var settings = await _settingsService.GetWebsiteSettingsAsync() ?? new WebsiteSettings();
                var requests = await _rentalService.GetRentalRequestsAsync();
                var list = requests
                    .OrderByDescending(r => r.StatusChangedDate ?? r.CreatedDate)
                    .Take(50)
                    .Select(r =>
                    {
                        var name = r.ApplicantName ?? r.RequesterName ?? "Applicant";
                        var verify = VerifyMember(name, r.RequesterEmail, r.RequesterPhone, r.MemberStatus);
                        var matrix = !string.IsNullOrWhiteSpace(r.RenterType) 
                            ? r.RenterType 
                            : (r.MemberStatus ? "Member" : "Non-Member");

                        var effectiveEventDate = r.EventDate != default ? r.EventDate : (r.RequestedDate != default ? r.RequestedDate : DateTime.Today);
                        var effectiveCreatedDate = r.CreatedDate != default ? r.CreatedDate : (r.RequestedDate != default ? r.RequestedDate : DateTime.Today);

                        return new
                        {
                            r.Id,
                            ApplicantName = name,
                            RequesterPhone = r.RequesterPhone ?? "",
                            RequesterEmail = r.RequesterEmail ?? "",
                            EventDate = effectiveEventDate,
                            CreatedAt = effectiveCreatedDate,
                            r.EventType,
                            r.StartTime,
                            r.EndTime,
                            r.RoomSelected,
                            r.GuestCount,
                            r.TotalPrice,
                            r.SecurityDepositAmount,
                            RequireSecurityDeposit = settings.RequireSecurityDeposit,
                            r.AmountPaid,
                            AmountWaived = (r.PaymentMethod == "Waived" && r.AmountPaid == 0 && r.IsPaid) ? (double)r.TotalPrice : 0.0,
                            r.IsPaid,
                            Status = string.IsNullOrWhiteSpace(r.Status) ? "Pending" : r.Status,
                            r.BartenderRequested,
                            r.KitchenUsage,
                            r.AvEquipmentUsage,
                            AdminNotes = r.InternalNotes ?? "",
                            MatrixSelected = matrix,
                            PreferredContactMethod = r.PreferredContactMethod,
                            RequestPhoneCall = r.RequestPhoneCall,
                            EventDescription = r.EventDescription ?? "",
                            IsVerifiedMember = verify.isVerified,
                            VerifiedMemberId = verify.memberId,
                            MemberVerificationText = verify.statusText,
                            MemberVerificationBadge = verify.badgeType,
                            PossibleMembers = verify.candidates
                        };
                    })
                    .ToList();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch rental requests: " + ex.Message });
            }
        }

        public class ApprovalActionRequest
        {
            public string? Notes { get; set; }
        }

        [HttpPost("mobile/approve/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> ApproveRental(int id, [FromBody] ApprovalActionRequest? body)
        {
            try
            {
                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var notes = !string.IsNullOrWhiteSpace(body?.Notes) ? body.Notes : "Approved via GFC Connect";
                
                var existing = await _rentalService.GetRentalRequestAsync(id);
                if (existing == null)
                {
                    return NotFound(new { error = "Rental request not found." });
                }

                var success = await _rentalService.ApproveRentalRequestAsync(id, notes, username);
                if (success)
                {
                    return Ok(new { success = true, message = "Rental approved successfully." });
                }
                return BadRequest(new { error = "Could not approve rental request. The date may already have another approved booking (double-booking blocked)." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error approving rental: " + ex.Message });
            }
        }

        [HttpPost("mobile/deny/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DenyRental(int id, [FromBody] ApprovalActionRequest? body)
        {
            try
            {
                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var notes = !string.IsNullOrWhiteSpace(body?.Notes) ? body.Notes : "Denied via GFC Connect";
                var success = await _rentalService.DenyRentalRequestAsync(id, notes, username);
                if (success)
                {
                    return Ok(new { success = true, message = "Rental request denied." });
                }
                return BadRequest(new { error = "Could not deny rental request." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error denying rental: " + ex.Message });
            }
        }

        [HttpPost("mobile/cancel/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CancelRental(int id, [FromBody] ApprovalActionRequest? body)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var reason = !string.IsNullOrWhiteSpace(body?.Notes) ? body.Notes : "Cancelled via GFC Connect";

                request.Status = "Cancelled";
                request.StatusChangedBy = username;
                request.StatusChangedDate = DateTime.UtcNow;
                request.InternalNotes = $"{request.InternalNotes}\n[{DateTime.Now:g}] CANCELLED by {username}: {reason}".Trim();

                await _rentalService.UpdateRentalRequestAsync(request);

                // Free up the calendar date
                var eventDate = request.EventDate != default ? request.EventDate : request.RequestedDate;
                if (eventDate != default)
                {
                    await _rentalService.UpdateCalendarAvailabilityAsync(eventDate, "Available");
                }

                return Ok(new { success = true, message = "Rental booking cancelled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error cancelling rental: " + ex.Message });
            }
        }

        [HttpDelete("mobile/delete/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DeleteRental(int id)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                await _rentalService.DeleteRentalRequestAsync(id);
                return Ok(new { success = true, message = "Rental request deleted permanently." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error deleting rental: " + ex.Message });
            }
        }

        [HttpGet("mobile/verify-member")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult VerifyMemberLive([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? phone, [FromQuery] bool isMember = true)
        {
            try
            {
                var verify = VerifyMember(name, email, phone, isMember);
                return Ok(new
                {
                    isVerified = verify.isVerified,
                    memberId = verify.memberId,
                    statusText = verify.statusText,
                    badgeType = verify.badgeType,
                    candidates = verify.candidates
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error verifying member status: " + ex.Message });
            }
        }

        [HttpGet("mobile/detail/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetRentalDetail(int id)
        {
            try
            {
                var r = await _rentalService.GetRentalRequestAsync(id);
                if (r == null) return NotFound(new { error = "Rental request not found." });

                var name = r.ApplicantName ?? r.RequesterName ?? "Applicant";
                var isClaimedMember = r.MemberStatus || 
                    (!string.IsNullOrWhiteSpace(r.RenterType) && r.RenterType.Contains("Member", StringComparison.OrdinalIgnoreCase) && !r.RenterType.Contains("Non", StringComparison.OrdinalIgnoreCase));
                var verify = VerifyMember(name, r.RequesterEmail, r.RequesterPhone, isClaimedMember);
                var matrix = !string.IsNullOrWhiteSpace(r.RenterType) 
                    ? r.RenterType 
                    : (isClaimedMember ? "Member" : "Non-Member");

                var effectiveEventDate = r.EventDate != default ? r.EventDate : (r.RequestedDate != default ? r.RequestedDate : DateTime.Today);

                var settings = await _settingsService.GetWebsiteSettingsAsync();
                var tierCards = (settings?.GetTierCardsList() ?? WebsiteSettings.GetDefaultTierCards()).Where(c => c.IsEnabled).ToList();
                var availableMatrixTiers = tierCards.Select(c => new
                {
                    Id = c.Id,
                    Title = c.Title,
                    Subtitle = c.Subtitle,
                    AssociatedRenterType = c.AssociatedRenterType,
                    ThemeColor = c.ThemeColor,
                    RateForDate = (double)GetCardRateForDate(c, effectiveEventDate),
                    IsAvailableForDate = IsCardAvailableForDate(c, effectiveEventDate),
                    IsSelected = string.Equals(matrix, c.Title, StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(matrix, c.Id, StringComparison.OrdinalIgnoreCase) ||
                                 string.Equals(matrix, c.AssociatedRenterType, StringComparison.OrdinalIgnoreCase)
                }).ToList();

                var payments = (await _rentalService.GetPaymentsForRequestAsync(id)).ToList();
                decimal actualPaid = payments
                    .Where(p => p.PaymentType != "Waived" && p.PaymentMethod != "Waived")
                    .Sum(p => p.PaymentType == "Refund" ? -p.Amount : p.Amount);
                decimal amountWaived = payments
                    .Where(p => p.PaymentType == "Waived" || p.PaymentMethod == "Waived")
                    .Sum(p => p.Amount);

                return Ok(new
                {
                    r.Id,
                    ApplicantName = name,
                    r.RequesterName,
                    r.RequesterEmail,
                    r.RequesterPhone,
                    r.RequesterAddress,
                    r.RequesterCity,
                    r.RequesterState,
                    r.RequesterZip,
                    EventDate = effectiveEventDate,
                    r.AlternateEventDate,
                    r.RoomSelected,
                    r.EventType,
                    r.EventDescription,
                    r.StartTime,
                    r.EndTime,
                    r.RenterType,
                    r.MemberStatus,
                    r.GuestCount,
                    r.RulesAgreed,
                    r.BartenderRequested,
                    r.KitchenUsage,
                    r.AvEquipmentUsage,
                    r.SecurityDepositPaid,
                    SecurityDepositAmount = (settings?.RequireSecurityDeposit == true ? r.SecurityDepositAmount : 0m),
                    RequireSecurityDeposit = settings?.RequireSecurityDeposit ?? true,
                    r.TotalPrice,
                    AmountPaid = (double)Math.Max(0, actualPaid),
                    AmountWaived = (double)amountWaived,
                    r.IsPaid,
                    r.PaymentMethod,
                    Status = string.IsNullOrWhiteSpace(r.Status) ? "Pending" : r.Status,
                    r.ApprovedBy,
                    r.ApprovalDate,
                    r.DeniedBy,
                    r.DenialDate,
                    r.StatusChangedBy,
                    r.StatusChangedDate,
                    InternalNotes = r.InternalNotes ?? "",
                    r.CreatedDate,
                    CreatedAt = r.CreatedDate != default ? r.CreatedDate : (r.RequestedDate != default ? r.RequestedDate : DateTime.Today),
                    MatrixSelected = matrix,
                    PreferredContactMethod = r.PreferredContactMethod,
                    RequestPhoneCall = r.RequestPhoneCall,
                    IsVerifiedMember = verify.isVerified,
                    VerifiedMemberId = verify.memberId,
                    MemberVerificationText = verify.statusText,
                    MemberVerificationBadge = verify.badgeType,
                    PossibleMembers = verify.candidates,
                    AvailableMatrixTiers = availableMatrixTiers,
                    AvailableAddons = (settings?.GetAddonsList() ?? RentalPricingDefaults.GetDefaultAddons())
                        .Where(a => a.IsActive)
                        .Select(a => new
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Fee = (double)a.Fee,
                            IsActive = a.IsActive,
                            IsSelected = (a.Id == "addon_bar" || a.Name.Contains("Bar", StringComparison.OrdinalIgnoreCase)) ? r.BartenderRequested :
                                         (a.Id == "addon_kitchen" || a.Name.Contains("Kitchen", StringComparison.OrdinalIgnoreCase)) ? r.KitchenUsage :
                                         (a.Id == "addon_av" || a.Name.Contains("AV", StringComparison.OrdinalIgnoreCase) || a.Name.Contains("Sound", StringComparison.OrdinalIgnoreCase)) ? r.AvEquipmentUsage : false
                        }).ToList(),
                    AvailableDaySchedules = (settings?.GetDaySchedulesList() ?? WebsiteSettings.GetDefaultDaySchedules())
                        .Select(d => new
                        {
                            DayOfWeek = (int)d.Day,
                            DayName = d.Day.ToString(),
                            IsAvailableForRentals = d.IsAvailableForRentals,
                            Slots = (d.Slots ?? d.TimeSlots ?? new List<DayScheduleSlotConfig>())
                                .Where(s => s.IsActive)
                                .Select(s => new
                                {
                                    Name = s.Name,
                                    StartTime = s.StartTime,
                                    EndTime = s.EndTime,
                                    IsActive = s.IsActive
                                }).ToList()
                        }).ToList(),
                    ModificationReasonPresets = settings?.GetModificationReasonPresetsList() ?? WebsiteSettings.GetDefaultModificationReasonPresets()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load rental details: " + ex.Message });
            }
        }

        public class UpdateRentalPayload
        {
            public string? ApplicantName { get; set; }
            public string? RequesterPhone { get; set; }
            public string? RequesterEmail { get; set; }
            public string? RequesterAddress { get; set; }
            public DateTime? EventDate { get; set; }
            public string? EventType { get; set; }
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public string? RoomSelected { get; set; }
            public int? GuestCount { get; set; }
            public decimal? TotalPrice { get; set; }
            public decimal? SecurityDepositAmount { get; set; }
            public decimal? AmountPaid { get; set; }
            public bool? IsPaid { get; set; }
            public bool? BartenderRequested { get; set; }
            public bool? KitchenUsage { get; set; }
            public bool? AvEquipmentUsage { get; set; }
            public string? Status { get; set; }
            public string? InternalNotes { get; set; }
            public string? RenterType { get; set; }
            public string? MatrixSelected { get; set; }
            public bool SendUpdateEmail { get; set; } = false;
            public string? ChangeReasonNote { get; set; }
        }

        [HttpPost("mobile/update/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> UpdateRental(int id, [FromBody] UpdateRentalPayload payload)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var matrix = payload.MatrixSelected ?? payload.RenterType;
                var targetEmail = !string.IsNullOrWhiteSpace(payload.RequesterEmail) ? payload.RequesterEmail.Trim() : request.RequesterEmail;

                // Build structured Audit Trail diff list and changelog matching webapp standard
                var changeDiffs = new List<string>();
                var nowStamp = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                if (!string.IsNullOrWhiteSpace(matrix) && !string.Equals(request.RenterType, matrix, StringComparison.OrdinalIgnoreCase))
                    changeDiffs.Add($"Pricing Tier: {request.RenterType ?? (request.MemberStatus ? "Members" : "Non-Members")} ➔ {matrix}");

                if (payload.EventDate.HasValue && payload.EventDate.Value.Date != request.EventDate.Date)
                    changeDiffs.Add($"Event Date: {request.EventDate:yyyy-MM-dd} ➔ {payload.EventDate.Value:yyyy-MM-dd}");

                if (!string.IsNullOrWhiteSpace(payload.Status) && !string.Equals(request.Status, payload.Status, StringComparison.OrdinalIgnoreCase))
                    changeDiffs.Add($"Status: {request.Status} ➔ {payload.Status}");

                if (payload.TotalPrice.HasValue && Math.Abs(request.TotalPrice - payload.TotalPrice.Value) > 0.01m)
                    changeDiffs.Add($"Total Price: ${request.TotalPrice:N2} ➔ ${payload.TotalPrice.Value:N2}");

                if (payload.GuestCount.HasValue && request.GuestCount != payload.GuestCount.Value)
                    changeDiffs.Add($"Guest Count: {request.GuestCount} ➔ {payload.GuestCount.Value}");

                if (!string.IsNullOrWhiteSpace(payload.RoomSelected) && !string.Equals(request.RoomSelected, payload.RoomSelected, StringComparison.OrdinalIgnoreCase))
                    changeDiffs.Add($"Room: {request.RoomSelected ?? "Function Hall"} ➔ {payload.RoomSelected}");

                if (payload.BartenderRequested.HasValue && request.BartenderRequested != payload.BartenderRequested.Value)
                    changeDiffs.Add($"Bartender Service: {(payload.BartenderRequested.Value ? "Added" : "Removed")}");

                if (payload.KitchenUsage.HasValue && request.KitchenUsage != payload.KitchenUsage.Value)
                    changeDiffs.Add($"Kitchen Access: {(payload.KitchenUsage.Value ? "Added" : "Removed")}");

                if (payload.AvEquipmentUsage.HasValue && request.AvEquipmentUsage != payload.AvEquipmentUsage.Value)
                    changeDiffs.Add($"A/V Equipment: {(payload.AvEquipmentUsage.Value ? "Added" : "Removed")}");

                if (payload.IsPaid.HasValue && request.IsPaid != payload.IsPaid.Value)
                    changeDiffs.Add($"Payment Status: {(payload.IsPaid.Value ? "Paid" : "Unpaid")}");

                if (!string.IsNullOrWhiteSpace(payload.RequesterEmail) && !string.Equals(request.RequesterEmail, payload.RequesterEmail.Trim(), StringComparison.OrdinalIgnoreCase))
                    changeDiffs.Add($"Email Address: {request.RequesterEmail ?? "None"} ➔ {payload.RequesterEmail.Trim()}");

                var auditSb = new System.Text.StringBuilder();
                auditSb.AppendLine($"[{nowStamp} by {username}]");
                foreach (var diff in changeDiffs)
                {
                    auditSb.AppendLine($"• {diff}");
                }

                if (!string.IsNullOrWhiteSpace(payload.ChangeReasonNote))
                {
                    auditSb.AppendLine($"• Reason / Note: {payload.ChangeReasonNote.Trim()}");
                }

                if (payload.SendUpdateEmail && !string.IsNullOrWhiteSpace(targetEmail))
                {
                    auditSb.AppendLine($"• Email Notice: Confirmation email dispatched to {targetEmail}");
                }

                if (changeDiffs.Count > 0 || !string.IsNullOrWhiteSpace(payload.ChangeReasonNote))
                {
                    auditSb.AppendLine();
                    request.InternalNotes = (auditSb.ToString() + (request.InternalNotes ?? "")).Trim();
                }
                else if (payload.InternalNotes != null)
                {
                    request.InternalNotes = payload.InternalNotes;
                }

                var targetDate = payload.EventDate ?? request.EventDate;
                var targetStart = payload.StartTime ?? request.StartTime;
                var targetEnd = payload.EndTime ?? request.EndTime;
                var targetRoom = payload.RoomSelected ?? request.RoomSelected;

                var isDateTimeChanged = (payload.EventDate.HasValue && payload.EventDate.Value.Date != request.EventDate.Date) ||
                                       (!string.IsNullOrWhiteSpace(payload.StartTime) && !string.Equals(payload.StartTime, request.StartTime, StringComparison.OrdinalIgnoreCase)) ||
                                       (!string.IsNullOrWhiteSpace(payload.EndTime) && !string.Equals(payload.EndTime, request.EndTime, StringComparison.OrdinalIgnoreCase)) ||
                                       (!string.IsNullOrWhiteSpace(payload.RoomSelected) && !string.Equals(payload.RoomSelected, request.RoomSelected, StringComparison.OrdinalIgnoreCase));

                if (isDateTimeChanged)
                {
                    var allUnavailable = await _rentalService.GetUnavailableDatesAsync();
                    var dayEvents = allUnavailable.Where(d => 
                    {
                        if (d.Date.Date != targetDate.Date) return false;
                        if (d.Id != 0 && d.Id == id) return false;
                        var desc = d.EventType ?? "";
                        if (!string.IsNullOrWhiteSpace(request.ApplicantName) && desc.Contains(request.ApplicantName, StringComparison.OrdinalIgnoreCase)) return false;
                        if (!string.IsNullOrWhiteSpace(request.RequesterName) && desc.Contains(request.RequesterName, StringComparison.OrdinalIgnoreCase)) return false;
                        return true;
                    }).ToList();

                    var isSecondaryRoom = targetRoom != null && (targetRoom.Contains("Office", StringComparison.OrdinalIgnoreCase) || targetRoom.Contains("Board", StringComparison.OrdinalIgnoreCase) || targetRoom.Contains("Lounge", StringComparison.OrdinalIgnoreCase));

                    if (dayEvents.Any())
                    {
                        var locName = targetRoom ?? "Function Hall";
                        var conflicts = dayEvents.Where(ev =>
                        {
                            var desc = ev.EventType ?? "";
                            var evIsSecondary = desc.Contains("Office", StringComparison.OrdinalIgnoreCase) || desc.Contains("Board", StringComparison.OrdinalIgnoreCase) || desc.Contains("Lounge", StringComparison.OrdinalIgnoreCase);

                            if (isSecondaryRoom && !evIsSecondary) return false;
                            if (!isSecondaryRoom && evIsSecondary) return false;

                            if (string.IsNullOrWhiteSpace(ev.EventTime) || ev.IsFullDay) return true;
                            if (string.IsNullOrWhiteSpace(targetStart) || string.IsNullOrWhiteSpace(targetEnd)) return true;

                            var startMin = ParseTimeToMinutes(targetStart);
                            var endMin = ParseTimeToMinutes(targetEnd);
                            var parts = ev.EventTime.Split(new[] { "-", "to", "–" }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 2)
                            {
                                var evSt = ParseTimeToMinutes(parts[0].Trim());
                                var evEt = ParseTimeToMinutes(parts[1].Trim());
                                return (startMin < evEt && endMin > evSt);
                            }
                            return true;
                        }).ToList();

                        if (conflicts.Any())
                        {
                            var first = conflicts.First();
                            var desc = first.EventType ?? "Existing Booking / Event";
                            var timeStr = !string.IsNullOrWhiteSpace(first.EventTime) ? $" ({first.EventTime})" : " (Full Day)";
                            return BadRequest(new { error = $"Scheduling Conflict: '{desc}'{timeStr} in {locName} already exists on this date/time." });
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(payload.ApplicantName))
                {
                    request.ApplicantName = payload.ApplicantName;
                    request.RequesterName = payload.ApplicantName;
                }
                if (payload.RequesterPhone != null) request.RequesterPhone = payload.RequesterPhone;
                if (!string.IsNullOrWhiteSpace(payload.RequesterEmail)) request.RequesterEmail = payload.RequesterEmail.Trim();
                if (payload.RequesterAddress != null) request.RequesterAddress = payload.RequesterAddress;
                if (payload.EventDate.HasValue) request.EventDate = payload.EventDate.Value;
                if (!string.IsNullOrWhiteSpace(payload.EventType)) request.EventType = payload.EventType;
                if (!string.IsNullOrWhiteSpace(payload.StartTime)) request.StartTime = payload.StartTime;
                if (!string.IsNullOrWhiteSpace(payload.EndTime)) request.EndTime = payload.EndTime;
                if (!string.IsNullOrWhiteSpace(payload.RoomSelected)) request.RoomSelected = payload.RoomSelected;
                if (payload.GuestCount.HasValue) request.GuestCount = payload.GuestCount.Value;
                if (payload.TotalPrice.HasValue) request.TotalPrice = payload.TotalPrice.Value;
                if (payload.SecurityDepositAmount.HasValue) request.SecurityDepositAmount = payload.SecurityDepositAmount.Value;
                if (payload.AmountPaid.HasValue) request.AmountPaid = payload.AmountPaid.Value;
                if (payload.IsPaid.HasValue) request.IsPaid = payload.IsPaid.Value;
                if (payload.BartenderRequested.HasValue) request.BartenderRequested = payload.BartenderRequested.Value;
                if (payload.KitchenUsage.HasValue) request.KitchenUsage = payload.KitchenUsage.Value;
                if (payload.AvEquipmentUsage.HasValue) request.AvEquipmentUsage = payload.AvEquipmentUsage.Value;

                if (!string.IsNullOrWhiteSpace(payload.Status))
                {
                    if (!string.Equals(request.Status, payload.Status, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(payload.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                        {
                            request.ApprovedBy = username;
                            request.ApprovalDate = DateTime.UtcNow;
                        }
                        else if (string.Equals(payload.Status, "Denied", StringComparison.OrdinalIgnoreCase))
                        {
                            request.DeniedBy = username;
                            request.DenialDate = DateTime.UtcNow;
                        }
                        else if (string.Equals(payload.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            request.ApprovedBy = null;
                            request.ApprovalDate = null;
                            request.DeniedBy = null;
                            request.DenialDate = null;
                        }
                    }
                    request.Status = payload.Status;
                }

                if (!string.IsNullOrWhiteSpace(matrix))
                {
                    request.RenterType = matrix;
                    request.MemberStatus = matrix.Contains("Member", StringComparison.OrdinalIgnoreCase) && !matrix.Contains("Non", StringComparison.OrdinalIgnoreCase);
                }

                request.StatusChangedBy = username;
                request.StatusChangedDate = DateTime.UtcNow;

                var updated = await _rentalService.UpdateRentalRequestAsync(request);
                if (updated)
                {
                    bool emailSent = false;
                    if (payload.SendUpdateEmail && !string.IsNullOrWhiteSpace(targetEmail))
                    {
                        try
                        {
                            var settings = await _settingsService.GetWebsiteSettingsAsync();
                            if (settings != null)
                            {
                                var subject = $"Updated Booking Information: {request.EventType ?? "Hall Rental"} on {request.EventDate:MM/dd/yyyy}";
                                var sb = new System.Text.StringBuilder();
                                sb.AppendLine($"<div style='font-family: Arial, sans-serif; color: #1e293b; max-width: 600px; padding: 16px;'>");
                                sb.AppendLine($"<h2 style='color: #0f172a; margin-bottom: 8px;'>Gloucester Fraternity Club</h2>");
                                sb.AppendLine($"<p>Dear {request.ApplicantName ?? "Applicant"},</p>");
                                sb.AppendLine($"<p>Your hall rental booking request specifications have been updated. Please review the current details below:</p>");

                                if (changeDiffs.Count > 0)
                                {
                                    sb.AppendLine($"<div style='background: #f1f5f9; border-left: 4px solid #0077b6; padding: 12px 16px; margin: 16px 0; border-radius: 4px;'>");
                                    sb.AppendLine($"<strong style='color: #0f172a; display: block; margin-bottom: 6px;'>Summary of Modifications:</strong>");
                                    sb.AppendLine($"<ul style='margin: 0; padding-left: 20px; line-height: 1.6;'>");
                                    foreach (var diff in changeDiffs)
                                    {
                                        sb.AppendLine($"<li>{diff}</li>");
                                    }
                                    sb.AppendLine($"</ul>");
                                    sb.AppendLine($"</div>");
                                }

                                if (!string.IsNullOrWhiteSpace(payload.ChangeReasonNote))
                                {
                                    sb.AppendLine($"<div style='background: #fffbeb; border-left: 4px solid #f59e0b; padding: 12px 16px; margin: 16px 0; border-radius: 4px;'>");
                                    sb.AppendLine($"<strong style='color: #92400e;'>Staff Note:</strong> {payload.ChangeReasonNote.Trim()}");
                                    sb.AppendLine($"</div>");
                                }

                                sb.AppendLine($"<div style='background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 16px; margin: 16px 0;'>");
                                sb.AppendLine($"<strong style='color: #334155; display: block; margin-bottom: 8px;'>Current Booking Specifications:</strong>");
                                sb.AppendLine($"<ul style='list-style: none; padding: 0; margin: 0; line-height: 1.8;'>");
                                sb.AppendLine($"<li><strong>Status:</strong> {request.Status}</li>");
                                sb.AppendLine($"<li><strong>Pricing Tier:</strong> {request.RenterType ?? (request.MemberStatus ? "Members" : "Non-Members")}</li>");
                                sb.AppendLine($"<li><strong>Event Date:</strong> {request.EventDate:dddd, MMMM dd, yyyy}</li>");
                                if (!string.IsNullOrWhiteSpace(request.StartTime) && !string.IsNullOrWhiteSpace(request.EndTime))
                                    sb.AppendLine($"<li><strong>Time Window:</strong> {request.StartTime} - {request.EndTime}</li>");
                                sb.AppendLine($"<li><strong>Facility Room:</strong> {request.RoomSelected ?? "Function Hall"}</li>");
                                if (request.GuestCount > 0)
                                    sb.AppendLine($"<li><strong>Estimated Guests:</strong> {request.GuestCount}</li>");
                                sb.AppendLine($"<li><strong>Rental Quote:</strong> ${request.TotalPrice:N2}</li>");
                                if (request.SecurityDepositAmount > 0)
                                    sb.AppendLine($"<li><strong>Refundable Security Deposit:</strong> ${request.SecurityDepositAmount:N2}</li>");
                                foreach (var addon in (settings?.GetAddonsList() ?? RentalPricingDefaults.GetDefaultAddons()).Where(a => a.IsActive))
                                {
                                    var isIncluded = (addon.Id == "addon_bar" || addon.Name.Contains("Bar", StringComparison.OrdinalIgnoreCase)) ? request.BartenderRequested :
                                                     (addon.Id == "addon_kitchen" || addon.Name.Contains("Kitchen", StringComparison.OrdinalIgnoreCase)) ? request.KitchenUsage :
                                                     (addon.Id == "addon_av" || addon.Name.Contains("AV", StringComparison.OrdinalIgnoreCase) || addon.Name.Contains("Sound", StringComparison.OrdinalIgnoreCase)) ? request.AvEquipmentUsage : false;
                                    sb.AppendLine($"<li><strong>{addon.Name}:</strong> {(isIncluded ? "Yes (Included)" : "No")}</li>");
                                }
                                sb.AppendLine($"</ul>");
                                sb.AppendLine($"</div>");
                                sb.AppendLine($"<p>If you have any questions or would like to discuss these changes, please contact the Gloucester Fraternity Club at {settings.ClubPhone ?? "(978) 283-2889"}.</p>");
                                sb.AppendLine($"<hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0 10px 0;'/>");
                                sb.AppendLine($"<small style='color: #94a3b8;'>Gloucester Fraternity Club &bull; 27 Webster Street, Gloucester, MA 01930 &bull; (978) 283-2889</small>");
                                sb.AppendLine($"</div>");

                                var emailResult = await _emailDispatcher.SendRentalEmailAsync(settings, targetEmail, subject, sb.ToString(), settings.RentalEmailCc);
                                emailSent = emailResult.Success;
                            }
                        }
                        catch (Exception emailEx)
                        {
                            Console.WriteLine($"Warning: Failed to dispatch update email: {emailEx.Message}");
                        }
                    }

                    return Ok(new { success = true, message = "Rental updated successfully.", emailSent });
                }
                return BadRequest(new { error = "Could not update rental request." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error updating rental: " + ex.Message });
            }
        }

        public class RecordPaymentPayload
        {
            public decimal Amount { get; set; }
            public string? PaymentMethod { get; set; } // Cash, Check, Credit Card, Venmo, Online, Waived
            public string? Note { get; set; }
            public bool MarkAsDeposit { get; set; }
            public bool IsWaived { get; set; }
        }

        [HttpPost("mobile/record-payment/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] RecordPaymentPayload payload)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var nowStamp = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                var currentPaid = request.AmountPaid;

                // Back-fill legacy payments that were recorded only on the request (no ledger rows)
                var existingLedger = (await _rentalService.GetPaymentsForRequestAsync(id)).ToList();
                var ledgerSum = existingLedger.Sum(p => p.PaymentType == "Refund" ? -p.Amount : p.Amount);
                if (currentPaid > ledgerSum)
                {
                    await _rentalService.RecordPaymentAsync(new HallRentalPayment
                    {
                        HallRentalRequestId = id,
                        PaymentType = "Rental Fee",
                        Amount = currentPaid - ledgerSum,
                        PaymentDate = request.PaymentDate ?? DateTime.Today,
                        PaymentMethod = request.PaymentMethod ?? "Other",
                        RecordedBy = username,
                        Notes = "Prior payments recorded before ledger sync"
                    });
                }
                var isWaiveAction = payload.IsWaived || (payload.PaymentMethod?.Contains("Waive", StringComparison.OrdinalIgnoreCase) == true);
                var waiveAmt = payload.Amount > 0 ? payload.Amount : Math.Max(0, request.TotalPrice - currentPaid);

                if (isWaiveAction)
                {
                    // Waived amount satisfies booking balance, but is NOT money collected
                    var reasonStr = !string.IsNullOrWhiteSpace(payload.Note) ? $" • Reason/Note: {payload.Note.Trim()}" : "";
                    var auditLine = $"[{nowStamp} by {username}]\n• 🎁 Payment/Fee Waived: ${waiveAmt:N2} (Actual Cash Paid: ${currentPaid:N2} of ${request.TotalPrice:N2}){reasonStr}";
                    request.InternalNotes = $"{auditLine}\n\n{(request.InternalNotes ?? "")}".Trim();

                    if (payload.MarkAsDeposit)
                    {
                        request.SecurityDepositPaid = true;
                    }
                    if (string.IsNullOrWhiteSpace(request.PaymentMethod))
                    {
                        request.PaymentMethod = "Waived";
                    }
                }
                else
                {
                    var newPaid = currentPaid + payload.Amount;
                    request.AmountPaid = newPaid;

                    if (payload.MarkAsDeposit)
                    {
                        request.SecurityDepositPaid = true;
                    }

                    if (newPaid >= request.TotalPrice && request.TotalPrice > 0)
                    {
                        request.IsPaid = true;
                    }

                    if (!string.IsNullOrWhiteSpace(payload.PaymentMethod))
                    {
                        request.PaymentMethod = payload.PaymentMethod;
                    }

                    var methodStr = !string.IsNullOrWhiteSpace(payload.PaymentMethod) ? $" via {payload.PaymentMethod}" : "";
                    var noteStr = !string.IsNullOrWhiteSpace(payload.Note) ? $" • Note: {payload.Note.Trim()}" : "";
                    var auditLine = $"[{nowStamp} by {username}]\n• 💵 Payment Recorded: +${payload.Amount:N2}{methodStr} (Total Paid: ${newPaid:N2} of ${request.TotalPrice:N2}){noteStr}";
                    request.InternalNotes = $"{auditLine}\n\n{(request.InternalNotes ?? "")}".Trim();
                }

                request.StatusChangedBy = username;
                request.StatusChangedDate = DateTime.UtcNow;

                await _rentalService.UpdateRentalRequestAsync(request);

                // Record in the payments ledger so the webapp Hall Rentals page and audit log show it
                var ledgerAmount = isWaiveAction ? waiveAmt : payload.Amount;
                if (ledgerAmount > 0)
                {
                    await _rentalService.RecordPaymentAsync(new HallRentalPayment
                    {
                        HallRentalRequestId = id,
                        PaymentType = isWaiveAction ? "Waived" : (payload.MarkAsDeposit ? "Security Deposit" : "Rental Fee"),
                        Amount = ledgerAmount,
                        PaymentDate = DateTime.Today,
                        PaymentMethod = isWaiveAction ? "Waived" : (payload.PaymentMethod ?? "Other"),
                        RecordedBy = username,
                        Notes = isWaiveAction ? (!string.IsNullOrWhiteSpace(payload.Note) ? $"Fee Waived: {payload.Note}" : "Fee Waived") : payload.Note
                    });
                }

                var successMsg = isWaiveAction 
                    ? $"Payment waiver of ${waiveAmt:N2} applied successfully." 
                    : $"Payment of ${payload.Amount:N2} recorded successfully.";

                return Ok(new { 
                    success = true, 
                    message = successMsg, 
                    amountPaid = request.AmountPaid,
                    isPaid = request.IsPaid,
                    securityDepositPaid = request.SecurityDepositPaid
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error processing payment: " + ex.Message });
            }
        }

        public class PaymentReminderPayload
        {
            public string? CustomNote { get; set; }
        }

        [HttpPost("mobile/payment-reminder/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> SendPaymentReminder(int id, [FromBody] PaymentReminderPayload? payload)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var targetEmail = !string.IsNullOrWhiteSpace(request.RequesterEmail) ? request.RequesterEmail.Trim() : null;
                if (string.IsNullOrWhiteSpace(targetEmail))
                {
                    return BadRequest(new { error = "No email address is associated with this rental booking." });
                }

                var settings = await _settingsService.GetWebsiteSettingsAsync() ?? new WebsiteSettings();
                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var nowStamp = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                var applicantName = request.ApplicantName ?? request.RequesterName ?? "Applicant";
                var eventDate = request.EventDate != default ? request.EventDate : request.RequestedDate;
                var eventDateStr = eventDate.ToString("MMMM dd, yyyy");
                var totalQuoted = request.TotalPrice;
                var amountPaid = request.AmountPaid;
                var remainingBalance = Math.Max(0m, totalQuoted - amountPaid);
                var clubNameStr = string.IsNullOrWhiteSpace(settings.RentalFormSubtitle) ? "Gloucester Fraternity Club" : settings.RentalFormSubtitle;
                var clubPhoneStr = string.IsNullOrWhiteSpace(settings.ClubPhone) ? "(978) 283-2889" : settings.ClubPhone;

                string subject = !string.IsNullOrWhiteSpace(settings.PaymentReminderEmailSubject) 
                    ? settings.PaymentReminderEmailSubject 
                    : "Payment Reminder - {ClubName} Hall Rental for {EventDate}";
                subject = subject
                    .Replace("{ClubName}", clubNameStr)
                    .Replace("{ApplicantName}", applicantName)
                    .Replace("{EventDate}", eventDateStr);

                string defaultTemplate = @"Dear {ApplicantName},

This is a friendly reminder regarding your upcoming hall rental booking with {ClubName} on {EventDate}.

Payment Summary:
- Total Rental Amount: ${TotalPrice}
- Total Payments Received: ${AmountPaid}
- Outstanding Balance Due: ${RemainingBalance}

Please remit your outstanding balance as soon as possible to maintain your reserved date. If you have already submitted payment, please disregard this notice.

If you have any questions or need assistance, please contact us at {ClubPhone}.

Warm regards,
{ClubName} Hall Rental Committee";

                string body = !string.IsNullOrWhiteSpace(settings.PaymentReminderEmailBody) 
                    ? settings.PaymentReminderEmailBody 
                    : defaultTemplate;

                body = body
                    .Replace("{ApplicantName}", applicantName)
                    .Replace("{EventDate}", eventDateStr)
                    .Replace("{TotalPrice}", totalQuoted.ToString("N2"))
                    .Replace("{AmountPaid}", amountPaid.ToString("N2"))
                    .Replace("{RemainingBalance}", remainingBalance.ToString("N2"))
                    .Replace("{ClubPhone}", clubPhoneStr)
                    .Replace("{ClubName}", clubNameStr);

                var noteHtml = "";
                if (!string.IsNullOrWhiteSpace(payload?.CustomNote))
                {
                    noteHtml = $"<div style='background: #fffbeb; border-left: 4px solid #f59e0b; padding: 12px; margin: 16px 0; border-radius: 4px;'>" +
                               $"<strong style='color: #92400e;'>Special Note / Instructions:</strong>" +
                               $"<p style='color: #78350f; margin: 4px 0 0 0;'>{System.Net.WebUtility.HtmlEncode(payload.CustomNote.Trim())}</p>" +
                               $"</div>";
                }

                string formattedHtml = $"<div style='font-family: Arial, sans-serif; font-size: 15px; color: #1e293b; line-height: 1.6; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 8px; background: #ffffff;'>" +
                    $"<div style='border-bottom: 2px solid #f59e0b; padding-bottom: 12px; margin-bottom: 16px;'>" +
                    $"<h2 style='color: #b45309; margin: 0;'>Payment &amp; Balance Reminder</h2>" +
                    $"<p style='color: #64748b; margin: 4px 0 0 0; font-size: 13px;'>{clubNameStr} &bull; Hall Rentals</p>" +
                    $"</div>" +
                    $"<div style='white-space: pre-line;'>{System.Net.WebUtility.HtmlEncode(body)}</div>" +
                    noteHtml +
                    $"<hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0 10px 0;'/>" +
                    $"<small style='color: #94a3b8;'>Gloucester Fraternity Club &bull; 27 Webster Street, Gloucester, MA 01930 &bull; {clubPhoneStr}</small>" +
                    $"</div>";

                var result = await _emailDispatcher.SendRentalEmailAsync(settings, targetEmail, subject, formattedHtml, settings.RentalEmailCc);
                if (result.Success)
                {
                    var customNoteStr = !string.IsNullOrWhiteSpace(payload?.CustomNote) ? $" • Note: {payload.CustomNote.Trim()}" : "";
                    var auditEntry = $"[{nowStamp} by {username}]\n• ✉️ Payment Reminder Email Sent to {targetEmail}{customNoteStr}";
                    request.InternalNotes = $"{auditEntry}\n\n{(request.InternalNotes ?? "")}".Trim();
                    await _rentalService.UpdateRentalRequestAsync(request);

                    return Ok(new { success = true, message = $"Payment reminder sent to {targetEmail}." });
                }

                return BadRequest(new { error = $"Failed to send email: {result.ErrorMessage}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error sending payment reminder: " + ex.Message });
            }
        }

        [HttpGet("mobile/unavailable-dates")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> GetUnavailableDates()
        {
            try
            {
                var dates = await _rentalService.GetUnavailableDatesAsync(includeRentalRequests: true);
                return Ok(dates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load unavailable dates: " + ex.Message });
            }
        }

        public class CreateClubEventPayload
        {
            public DateTime Date { get; set; }
            public string Reason { get; set; } = string.Empty;
            public string? Location { get; set; } = "Function Hall";
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public bool IsFullDay { get; set; } = true;
        }

        [HttpPost("mobile/club-event")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CreateClubEvent([FromBody] CreateClubEventPayload payload)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(payload.Reason))
                {
                    return BadRequest(new { error = "Event title or reason is required." });
                }

                var settings = await _settingsService.GetWebsiteSettingsAsync() ?? new WebsiteSettings();
                var primaryLoc = settings.ManagedRentalLocation ?? "Function Hall";
                var secondaryLoc = settings.SecondaryFlexibleLocation ?? "Office";
                var locName = !string.IsNullOrWhiteSpace(payload.Location) ? payload.Location.Trim() : primaryLoc;

                var start = !payload.IsFullDay ? payload.StartTime : null;
                var end = !payload.IsFullDay ? payload.EndTime : null;
                var description = $"Club Event ({locName}): {payload.Reason.Trim()}";

                // Server-side space-aware slot availability validation
                var targetDate = payload.Date.Date;
                var unavailable = await _rentalService.GetUnavailableDatesAsync(includeRentalRequests: true);
                var isSecondary = locName.Contains("Office", StringComparison.OrdinalIgnoreCase) ||
                                  (!string.IsNullOrWhiteSpace(settings.SecondaryFlexibleLocation) && locName.Contains(settings.SecondaryFlexibleLocation, StringComparison.OrdinalIgnoreCase));

                var dayEvents = unavailable.Where(d => {
                    if (d.Date.Date != targetDate) return false;
                    var desc = d.EventType ?? string.Empty;
                    var isEventInSecondary = desc.Contains("Office", StringComparison.OrdinalIgnoreCase) ||
                                             (!string.IsNullOrWhiteSpace(settings.SecondaryFlexibleLocation) && desc.Contains(settings.SecondaryFlexibleLocation, StringComparison.OrdinalIgnoreCase));

                    // If booking in secondary space, only check conflicts in secondary space
                    if (isSecondary)
                    {
                        return isEventInSecondary;
                    }
                    else
                    {
                        // If booking in Function Hall, check all bookings except ones exclusively in secondary space
                        return !isEventInSecondary;
                    }
                }).ToList();

                if (dayEvents.Any())
                {
                    if (payload.IsFullDay)
                    {
                        var firstConflict = dayEvents.First();
                        var conflictDesc = firstConflict.EventType ?? "Existing Booking / Event";
                        var conflictTime = !string.IsNullOrWhiteSpace(firstConflict.EventTime) ? $" ({firstConflict.EventTime})" : " (Full Day)";
                        return BadRequest(new { error = $"Cannot schedule full day event: Conflicting event in {locName} '{conflictDesc}'{conflictTime} already exists on this date." });
                    }
                    else if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
                    {
                        var startMin = ParseTimeToMinutes(start);
                        var endMin = ParseTimeToMinutes(end);

                        foreach (var ev in dayEvents)
                        {
                            if (ev.IsFullDay || string.IsNullOrWhiteSpace(ev.EventTime))
                            {
                                var conflictDesc = ev.EventType ?? "Full Day Booking";
                                return BadRequest(new { error = $"Time conflict: '{conflictDesc}' in {locName} is already scheduled for the full day on this date." });
                            }

                            var parts = ev.EventTime.Split('-', StringSplitOptions.TrimEntries);
                            if (parts.Length == 2)
                            {
                                var evStart = ParseTimeToMinutes(parts[0]);
                                var evEnd = ParseTimeToMinutes(parts[1]);

                                if (evStart >= 0 && evEnd >= 0 && startMin < evEnd && endMin > evStart)
                                {
                                    var conflictDesc = ev.EventType ?? "Booking";
                                    return BadRequest(new { error = $"Time slot conflict: The requested hours ({start} - {end}) in {locName} overlap with '{conflictDesc}' ({ev.EventTime})." });
                                }
                            }
                        }
                    }
                }

                await _rentalService.AddBlackoutDateAsync(payload.Date, description, start, end);
                return Ok(new { success = true, message = $"Club event '{payload.Reason}' scheduled in {locName} for {payload.Date:MMM dd, yyyy}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error creating club event: " + ex.Message });
            }
        }

        public class UpdateClubEventPayload
        {
            public DateTime Date { get; set; }
            public string Reason { get; set; } = string.Empty;
            public string Location { get; set; } = "Function Hall";
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public bool IsFullDay { get; set; } = true;
        }

        [HttpPut("mobile/club-event/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> UpdateClubEvent(int id, [FromBody] UpdateClubEventPayload payload)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(payload.Reason))
                {
                    return BadRequest(new { error = "Event title or reason is required." });
                }

                var settings = await _settingsService.GetWebsiteSettingsAsync() ?? new WebsiteSettings();
                var primaryLoc = settings.ManagedRentalLocation ?? "Function Hall";
                var secondaryLoc = settings.SecondaryFlexibleLocation ?? "Office";
                var locName = !string.IsNullOrWhiteSpace(payload.Location) ? payload.Location.Trim() : primaryLoc;

                var start = !payload.IsFullDay ? payload.StartTime : null;
                var end = !payload.IsFullDay ? payload.EndTime : null;
                var description = $"Club Event ({locName}): {payload.Reason.Trim()}";

                var targetDate = payload.Date.Date;
                var unavailable = await _rentalService.GetUnavailableDatesAsync(includeRentalRequests: true);
                var isSecondary = locName.Contains("Office", StringComparison.OrdinalIgnoreCase) ||
                                  (!string.IsNullOrWhiteSpace(settings.SecondaryFlexibleLocation) && locName.Contains(settings.SecondaryFlexibleLocation, StringComparison.OrdinalIgnoreCase));

                var dayEvents = unavailable.Where(d => {
                    if (d.Id == id) return false; // Exclude currently edited event from conflict check
                    if (d.Date.Date != targetDate) return false;
                    var desc = d.EventType ?? string.Empty;
                    var isEventInSecondary = desc.Contains("Office", StringComparison.OrdinalIgnoreCase) ||
                                             (!string.IsNullOrWhiteSpace(settings.SecondaryFlexibleLocation) && desc.Contains(settings.SecondaryFlexibleLocation, StringComparison.OrdinalIgnoreCase));

                    if (isSecondary)
                    {
                        return isEventInSecondary;
                    }
                    else
                    {
                        return !isEventInSecondary;
                    }
                }).ToList();

                if (dayEvents.Any())
                {
                    if (payload.IsFullDay)
                    {
                        var firstConflict = dayEvents.First();
                        var conflictDesc = firstConflict.EventType ?? "Existing Booking / Event";
                        var conflictTime = !string.IsNullOrWhiteSpace(firstConflict.EventTime) ? $" ({firstConflict.EventTime})" : " (Full Day)";
                        return BadRequest(new { error = $"Cannot schedule full day event: Conflicting event in {locName} '{conflictDesc}'{conflictTime} already exists on this date." });
                    }
                    else if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
                    {
                        var startMin = ParseTimeToMinutes(start);
                        var endMin = ParseTimeToMinutes(end);

                        foreach (var ev in dayEvents)
                        {
                            if (ev.IsFullDay || string.IsNullOrWhiteSpace(ev.EventTime))
                            {
                                var conflictDesc = ev.EventType ?? "Full Day Booking";
                                return BadRequest(new { error = $"Time conflict: '{conflictDesc}' in {locName} is already scheduled for the full day on this date." });
                            }

                            var parts = ev.EventTime.Split('-', StringSplitOptions.TrimEntries);
                            if (parts.Length == 2)
                            {
                                var evStart = ParseTimeToMinutes(parts[0]);
                                var evEnd = ParseTimeToMinutes(parts[1]);

                                if (evStart >= 0 && evEnd >= 0 && startMin < evEnd && endMin > evStart)
                                {
                                    var conflictDesc = ev.EventType ?? "Booking";
                                    return BadRequest(new { error = $"Time slot conflict: The requested hours ({start} - {end}) in {locName} overlap with '{conflictDesc}' ({ev.EventTime})." });
                                }
                            }
                        }
                    }
                }

                await _rentalService.UpdateBlackoutDateAsync(id, payload.Date, description, start ?? string.Empty, end ?? string.Empty);
                return Ok(new { success = true, message = $"Club event '{payload.Reason}' updated successfully in {locName}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error updating club event: " + ex.Message });
            }
        }

        [HttpDelete("mobile/club-event/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DeleteClubEvent(int id)
        {
            try
            {
                await _rentalService.RemoveBlackoutDateByIdAsync(id);
                return Ok(new { success = true, message = "Club event deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error deleting club event: " + ex.Message });
            }
        }

        private static int ParseTimeToMinutes(string? timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr)) return -1;
            timeStr = timeStr.Trim();
            if (DateTime.TryParse(timeStr, out var dt))
            {
                return dt.Hour * 60 + dt.Minute;
            }
            var parts = timeStr.Split(':', StringSplitOptions.TrimEntries);
            if (parts.Length >= 2 && int.TryParse(parts[0], out var h))
            {
                var minPart = parts[1].Split(' ', StringSplitOptions.TrimEntries)[0];
                if (int.TryParse(minPart, out var m))
                {
                    bool isPm = timeStr.EndsWith("PM", StringComparison.OrdinalIgnoreCase);
                    bool isAm = timeStr.EndsWith("AM", StringComparison.OrdinalIgnoreCase);
                    if (isPm && h < 12) h += 12;
                    if (isAm && h == 12) h = 0;
                    return h * 60 + m;
                }
            }
            return -1;
        }

        public class LogCorrespondencePayload
        {
            public string Type { get; set; } = "call"; // "call", "sms", "email", "note"
            public string? Outcome { get; set; } // "Spoke with Applicant", "Left Voicemail", "No Answer", "Sent"
            public string? Notes { get; set; }
        }

        public class ArchiveRentalPayload
        {
            public bool Archive { get; set; } = true;
            public string? Note { get; set; }
        }

        [HttpPost("mobile/archive/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> ArchiveRental(int id, [FromBody] ArchiveRentalPayload? payload)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var nowStamp = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                var shouldArchive = payload?.Archive ?? true;
                if (shouldArchive)
                {
                    request.Status = "Archived";
                    var noteStr = !string.IsNullOrWhiteSpace(payload?.Note) ? $" • Note: {payload.Note.Trim()}" : "";
                    var logEntry = $"[{nowStamp} by {username}]\n• 📁 Inquiry archived for FAQ & knowledgebase review{noteStr}";
                    request.InternalNotes = $"{logEntry}\n\n{(request.InternalNotes ?? "")}".Trim();
                }
                else
                {
                    request.Status = "Inquiry";
                    var logEntry = $"[{nowStamp} by {username}]\n• 📂 Inquiry unarchived and restored to active inquiries list";
                    request.InternalNotes = $"{logEntry}\n\n{(request.InternalNotes ?? "")}".Trim();
                }

                request.StatusChangedBy = username;
                request.StatusChangedDate = DateTime.UtcNow;

                await _rentalService.UpdateRentalRequestAsync(request);
                return Ok(new { success = true, message = shouldArchive ? "Inquiry archived successfully." : "Inquiry unarchived.", status = request.Status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error archiving inquiry: " + ex.Message });
            }
        }

        [HttpPost("mobile/log-correspondence/{id:int}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> LogCorrespondence(int id, [FromBody] LogCorrespondencePayload payload)
        {
            try
            {
                var request = await _rentalService.GetRentalRequestAsync(id);
                if (request == null) return NotFound(new { error = "Rental not found." });

                var rawName = User.Identity?.Name;
                var username = !string.IsNullOrWhiteSpace(rawName) ? $"{rawName} (GFC Connect)" : "Admin (GFC Connect)";
                var nowStamp = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");

                var icon = payload.Type?.ToLowerInvariant() switch
                {
                    "call" => "📞",
                    "sms" => "💬",
                    "email" => "✉️",
                    _ => "📝"
                };

                var typeLabel = payload.Type?.ToLowerInvariant() switch
                {
                    "call" => "Phone Call",
                    "sms" => "SMS / Text Message",
                    "email" => "Email Communication",
                    _ => "Note / In-Person"
                };

                var outcomeStr = !string.IsNullOrWhiteSpace(payload.Outcome) ? $" ({payload.Outcome})" : "";
                var notesStr = !string.IsNullOrWhiteSpace(payload.Notes) ? $"\n  Note: {payload.Notes.Trim()}" : "";

                var logEntry = $"[{nowStamp} by {username}]\n• {icon} {typeLabel}{outcomeStr}{notesStr}";
                request.InternalNotes = $"{logEntry}\n\n{(request.InternalNotes ?? "")}".Trim();

                if (string.Equals(request.Status, "Inquiry", StringComparison.OrdinalIgnoreCase))
                {
                    request.Status = "Responded";
                }

                request.StatusChangedBy = username;
                request.StatusChangedDate = DateTime.UtcNow;

                await _rentalService.UpdateRentalRequestAsync(request);
                return Ok(new { success = true, message = "Correspondence logged successfully.", internalNotes = request.InternalNotes, status = request.Status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error logging correspondence: " + ex.Message });
            }
        }
    }
}
