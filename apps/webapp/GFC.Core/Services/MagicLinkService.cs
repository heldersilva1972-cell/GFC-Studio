// [NEW]
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services
{
    public class MagicLinkService : IMagicLinkService
    {
        private readonly GfcDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<MagicLinkService> _logger;

        public MagicLinkService(
            GfcDbContext context,
            IUserRepository userRepository,
            IEmailService emailService,
            ISystemSettingsService systemSettingsService,
            IAuditLogger auditLogger,
            ILogger<MagicLinkService> logger)
        {
            _context = context;
            _userRepository = userRepository;
            _emailService = emailService;
            _systemSettingsService = systemSettingsService;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<RequestMagicLinkResult> RequestMagicLinkAsync(string email, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new RequestMagicLinkResult { Success = false, UserMessage = "Email address is required." };
            }

            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                // Do not reveal that the user does not exist.
                _logger.LogWarning("Magic link requested for non-existent email: {Email}", email);
                return new RequestMagicLinkResult { Success = true, UserMessage = "If an account with that email exists, a magic link has been sent." };
            }

            // Rate limit check: 60 seconds
            var rateLimit = DateTime.UtcNow.AddSeconds(-60);
            var recentToken = await _context.MagicLinkTokens
                .Where(t => t.UserId == user.UserId && t.CreatedAtUtc > rateLimit)
                .AnyAsync();

            if (recentToken)
            {
                return new RequestMagicLinkResult { Success = false, UserMessage = "A magic link was recently sent. Please wait a moment before requesting another." };
            }

            // Generate token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var magicLinkToken = new MagicLinkToken
            {
                UserId = user.UserId,
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(15),
                IpAddress = ipAddress
            };

            _context.MagicLinkTokens.Add(magicLinkToken);
            await _context.SaveChangesAsync();

            // Send email
            var systemSettings = await _systemSettingsService.GetSystemSettingsAsync();
            var primaryDomain = systemSettings.PrimaryDomain;
            var link = $"https://{primaryDomain}/auth/magic-login?token={token}";

            await _emailService.SendEmailAsync(email, "Your Magic Login Link", $"Click here to log in: {link}");

            _auditLogger.Log(user.UserId, "MagicLinkSent", $"Magic link sent to {email} for user {user.Username}.");

            return new RequestMagicLinkResult { Success = true, UserMessage = "If an account with that email exists, a magic link has been sent." };
        }

        public async Task<MagicLinkValidationResult> ValidateMagicLinkTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new MagicLinkValidationResult { Success = false, ErrorMessage = "Invalid or missing token." };
            }

            var magicLinkToken = await _context.MagicLinkTokens
                .Include(t => t.User)
                .SingleOrDefaultAsync(t => t.Token == token);

            if (magicLinkToken == null || magicLinkToken.IsUsed || magicLinkToken.ExpiresAtUtc < DateTime.UtcNow)
            {
                return new MagicLinkValidationResult { Success = false, ErrorMessage = "Invalid or expired link." };
            }

            magicLinkToken.IsUsed = true;
            await _context.SaveChangesAsync();

            return new MagicLinkValidationResult { Success = true, User = magicLinkToken.User };
        }
    }
}
