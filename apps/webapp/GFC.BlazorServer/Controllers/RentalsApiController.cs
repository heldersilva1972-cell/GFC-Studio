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
            if (string.IsNullOrWhiteSpace(name)) 
                return (false, null, "No applicant name", "NON_MEMBER", candidates);

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

            // 2. Match Full Combined / Normalized Name (handles suffixes and middle initials seamlessly)
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

            // 3. Match Phone
            if (match == null && cleanPhone.Length >= 7)
            {
                match = members.FirstOrDefault(m => 
                    !string.IsNullOrEmpty(m.Phone) && System.Text.RegularExpressions.Regex.Replace(m.Phone, @"[^\d]", "").EndsWith(cleanPhone.Length > 7 ? cleanPhone.Substring(cleanPhone.Length - 7) : cleanPhone));
            }

            // 4. Match Email
            if (match == null && cleanEmail.Length > 3 && cleanEmail.Contains("@"))
            {
                match = members.FirstOrDefault(m => string.Equals((m.Email ?? "").Trim(), cleanEmail, StringComparison.OrdinalIgnoreCase));
            }

            if (match != null)
            {
                var status = string.IsNullOrWhiteSpace(match.Status) ? "Active" : match.Status;
                var display = GetMemberDisplayName(match);
                return (true, match.MemberID, $"Verified: {display} #{match.MemberID} ({status})", "VERIFIED", candidates);
            }

            // 5. Search for Similar Candidate Members (considering Suffix)
            foreach (var m in members)
            {
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

            if (claimedMember)
            {
                var claimText = candidates.Any() 
                    ? $"Claimed Member ({candidates.Count} candidate match{(candidates.Count > 1 ? "es" : "")})"
                    : "Claimed Member (Not in Directory)";
                return (false, null, claimText, "UNVERIFIED_CLAIM", candidates);
            }

            return (false, null, "Non-Member", "NON_MEMBER", candidates);
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
            if (payload.BarService) addOns += (settings?.BartenderServiceFee ?? 100);
            if (payload.KitchenAccess) addOns += (settings?.KitchenFee ?? 0);
            if (payload.AvEquipment) addOns += (settings?.AvEquipmentFee ?? 0);

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

                        return new
                        {
                            r.Id,
                            ApplicantName = name,
                            RequesterPhone = r.RequesterPhone ?? "",
                            RequesterEmail = r.RequesterEmail ?? "",
                            EventDate = effectiveEventDate,
                            r.EventType,
                            r.StartTime,
                            r.EndTime,
                            r.RoomSelected,
                            r.GuestCount,
                            r.TotalPrice,
                            r.SecurityDepositAmount,
                            Status = string.IsNullOrWhiteSpace(r.Status) ? "Pending" : r.Status,
                            r.BartenderRequested,
                            r.KitchenUsage,
                            r.AvEquipmentUsage,
                            AdminNotes = r.InternalNotes ?? "",
                            MatrixSelected = matrix,
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
                var success = await _rentalService.ApproveRentalRequestAsync(id, notes, username);
                if (success)
                {
                    return Ok(new { success = true, message = "Rental approved successfully." });
                }
                return BadRequest(new { error = "Could not approve rental request." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error approving rental: " + ex.Message });
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
                var verify = VerifyMember(name, r.RequesterEmail, r.RequesterPhone, r.MemberStatus);
                var matrix = !string.IsNullOrWhiteSpace(r.RenterType) 
                    ? r.RenterType 
                    : (r.MemberStatus ? "Member" : "Non-Member");

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
                    r.SecurityDepositAmount,
                    r.TotalPrice,
                    r.AmountPaid,
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
                    MatrixSelected = matrix,
                    IsVerifiedMember = verify.isVerified,
                    VerifiedMemberId = verify.memberId,
                    MemberVerificationText = verify.statusText,
                    MemberVerificationBadge = verify.badgeType,
                    PossibleMembers = verify.candidates,
                    AvailableMatrixTiers = availableMatrixTiers,
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
                                sb.AppendLine($"<li><strong>Bar / Bartender Service:</strong> {(request.BartenderRequested ? "Yes (Included)" : "No")}</li>");
                                sb.AppendLine($"<li><strong>Kitchen Access:</strong> {(request.KitchenUsage ? "Yes (Included)" : "No")}</li>");
                                sb.AppendLine($"<li><strong>A/V Sound &amp; Equipment:</strong> {(request.AvEquipmentUsage ? "Yes (Included)" : "No")}</li>");
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
                    return Ok(new { success = true, message = "Rental denied successfully." });
                }
                return BadRequest(new { error = "Could not deny rental request." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error denying rental: " + ex.Message });
            }
        }
    }
}
