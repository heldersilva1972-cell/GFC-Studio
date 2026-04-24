using GFC.Core.DTOs;
using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoginHistoryRepository _loginHistoryRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IAuditLogger _auditLogger;
    private readonly IPasswordPolicy _passwordPolicy;
    private readonly IPagePermissionRepository _pagePermissionRepository;
    private readonly IBoardTermConfirmationService _boardTermConfirmationService;
    private readonly IDeviceTrustService _deviceTrustService; // [FIX] Now using Core Interface
    
    public event Action? PermissionsUpdated;

    // PERFORMANCE CACHE: Persists across circuits (Static)
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<int, HashSet<string>> _permissionCache = new();
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<int, List<GFC.Core.DTOs.MobilePermissionDto>> _userPermissionsCache = new();

    public UserManagementService(
        IUserRepository userRepository,
        IMemberRepository memberRepository,
        ILoginHistoryRepository loginHistoryRepository,
        IDuesRepository duesRepository,
        IBoardRepository boardRepository,
        IAuditLogger auditLogger,
        IPasswordPolicy passwordPolicy,
        IPagePermissionRepository pagePermissionRepository,
        IBoardTermConfirmationService boardTermConfirmationService,
        IDeviceTrustService deviceTrustService) // [FIX] Now using Core Interface
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _loginHistoryRepository = loginHistoryRepository ?? throw new ArgumentNullException(nameof(loginHistoryRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _passwordPolicy = passwordPolicy ?? throw new ArgumentNullException(nameof(passwordPolicy));
        _pagePermissionRepository = pagePermissionRepository ?? throw new ArgumentNullException(nameof(pagePermissionRepository));
        _boardTermConfirmationService = boardTermConfirmationService ?? throw new ArgumentNullException(nameof(boardTermConfirmationService));
        _deviceTrustService = deviceTrustService;
    }

    private void InvalidateGlobalSession(int userId)
    {
        try
        {
            // Clear local core caches first
            _permissionCache.TryRemove(userId, out _);
            _userPermissionsCache.TryRemove(userId, out _);
            _deviceTrustService.InvalidateUserSession(userId);

            // Use reflection to call the static method from the Blazor layer to clear the global memory cache
            var authStateProviderType = Type.GetType("GFC.BlazorServer.Services.CustomAuthenticationStateProvider, GFC.BlazorServer");
            if (authStateProviderType != null)
            {
                var invalidateMethod = authStateProviderType.GetMethod("InvalidateUser", 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                invalidateMethod?.Invoke(null, new object[] { userId });
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[UserManagement] Warning: Failed to invalidate global session for user {userId}: {ex.Message}");
        }
    }

    // ... (rest of methods)

    public void DeleteUser(int userId)
    {
        // [FIX] Cleanup caches and device trust before deleting user
        try
        {
            _deviceTrustService.ResetMobileSetupAsync(userId).GetAwaiter().GetResult();
            InvalidateGlobalSession(userId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[UserManagement] Warning: specific device/cache cleanup failed during user deletion: {ex.Message}");
        }

        _userRepository.DeleteUser(userId);
    }

    public async Task DeleteUserAsync(int userId)
    {
        // [FIX] Cleanup caches and device trust before deleting user (Async)
        try
        {
            await _deviceTrustService.ResetMobileSetupAsync(userId);
            InvalidateGlobalSession(userId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[UserManagement] Warning: specific device/cache cleanup failed during user deletion: {ex.Message}");
        }

        // Repository delete is currently sync, but that's okay as it's a fast db op usually.
        // If needed we can stick it in Task.Run or update repo to be async later.
        _userRepository.DeleteUser(userId);
    }

    public void ClearPermissionCache()
    {
        _permissionCache.Clear();
        _userPermissionsCache.Clear();
    }

    public List<UserListItemDto> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        var result = new List<UserListItemDto>();

        // OPTIMIZATION: Pre-fetch all members and board assignments in bulk to avoid N+1 queries
        var allMembers = _memberRepository.GetAllMembers().ToDictionary(m => m.MemberID, m => m);
        
        var currentYear = DateTime.Now.Year;
        var currentDirectors = new HashSet<int>();
        var prevDirectors = new HashSet<int>();
        
        try 
        {
            currentDirectors = _boardRepository.GetAssignmentsByYear(currentYear).Select(a => a.MemberID).ToHashSet();
            prevDirectors = _boardRepository.GetAssignmentsByYear(currentYear - 1).Select(a => a.MemberID).ToHashSet();
        }
        catch { /* Fallback to empty if table fails */ }

        var confirmation = _boardTermConfirmationService.GetConfirmation(currentYear);

        foreach (var user in users)
        {
            string? memberName = null;
            bool isDirector = false;
            
            if (user.MemberId.HasValue && allMembers.TryGetValue(user.MemberId.Value, out var member))
            {
                memberName = FormatMemberDisplayName(member);
                
                // Optimized director check using local HashSet
                isDirector = currentDirectors.Contains(user.MemberId.Value);
                if (!isDirector && confirmation == null)
                {
                    isDirector = prevDirectors.Contains(user.MemberId.Value);
                }
            }

            result.Add(new UserListItemDto(
                user.UserId,
                user.Username,
                user.IsAdmin,
                user.IsActive,
                user.MemberId,
                memberName,
                user.LastLoginDate,
                user.Notes,
                user.Email,
                isDirector));
        }

        return result;
    }

    public async Task<List<UserListItemDto>> GetUsersAsync()
    {
        return await Task.FromResult(GetAllUsers());
    }

    private HashSet<int> GetCurrentAndTransitionalDirectors()
    {
        var currentYear = DateTime.Now.Year;
        var currentDirectors = _boardRepository.GetAssignmentsByYear(currentYear)
            .Select(a => a.MemberID)
            .ToHashSet();
            
        var confirmation = _boardTermConfirmationService.GetConfirmation(currentYear);
        if (confirmation == null)
        {
            var prevYearDirectors = _boardRepository.GetAssignmentsByYear(currentYear - 1)
                .Select(a => a.MemberID);
            foreach (var id in prevYearDirectors) 
            {
                currentDirectors.Add(id);
            }
        }

        return currentDirectors;
    }

    public List<ActiveMemberDto> GetEligibleDirectorsForUserCreation()
    {
        var allMembers = _memberRepository.GetAllMembers();
        var currentDirectors = GetCurrentAndTransitionalDirectors();

        // Filter for members who are both active and currently on the board/directors list.
        var eligibleDirectors = allMembers
            .Where(m => IsActiveForDues(m) && currentDirectors.Contains(m.MemberID))
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Select(m => new ActiveMemberDto(
                m.MemberID,
                FormatMemberDisplayName(m),
                m.FirstName,
                m.LastName,
                m.Status))
            .ToList();

        return eligibleDirectors;
    }

    public List<ActiveMemberDto> GetEligibleMembersForUserCreation()
    {
        var allMembers = _memberRepository.GetAllMembers();
        var currentDirectors = GetCurrentAndTransitionalDirectors();

        // Filter for members who are active but NOT on the board/directors list.
        var eligibleMembers = allMembers
            .Where(m => IsActiveForDues(m) && !currentDirectors.Contains(m.MemberID))
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Select(m => new ActiveMemberDto(
                m.MemberID,
                FormatMemberDisplayName(m),
                m.FirstName,
                m.LastName,
                m.Status))
            .ToList();

        return eligibleMembers;
    }

    private static bool IsActiveForDues(Member member)
    {
        return (member.Status is "REGULAR" or "REGULAR-NP" or "LIFE" or "GUEST")
            && member.Status != "INACTIVE" 
            && member.Status != "DECEASED" 
            && member.Status != "REJECTED";
    }

    private static string FormatMemberDisplayName(Member member)
    {
        var suffix = !string.IsNullOrWhiteSpace(member.Suffix) ? $" {member.Suffix}" : "";
        var middle = !string.IsNullOrWhiteSpace(member.MiddleName) ? $" {member.MiddleName[0]}." : "";
        
        return $"{member.LastName}{suffix}, {member.FirstName}{middle}";
    }

    private bool HasPaidOrWaivedDuesOptimized(Member member, Dictionary<int, DuesPayment> duesLookup, int year, HashSet<int> directors)
    {
        // Life members are always waived
        if (member.Status == "LIFE") return true;
        
        // Board members are waived - check optimized hashset
        if (directors.Contains(member.MemberID)) return true;
        
        // Check if they have a paid or waived dues record
        if (duesLookup.TryGetValue(member.MemberID, out var dues))
        {
            if (dues.PaidDate.HasValue || 
                (dues.PaymentType != null && dues.PaymentType.Equals("WAIVED", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }
        
        return false;
    }

    public AppUser? GetUser(int userId)
    {
        return _userRepository.GetById(userId);
    }

    public async Task<AppUser?> GetUserAsync(int userId)
    {
        return await Task.FromResult(_userRepository.GetById(userId));
    }

    public int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null, bool mfaEnabled = false)
    {
        if (_userRepository.UsernameExists(username))
        {
            throw new InvalidOperationException($"Username '{username}' already exists.");
        }

        string passwordHash;
        if (string.IsNullOrWhiteSpace(password))
        {
            passwordHash = ""; // No password set yet
            passwordChangeRequired = true; // MUST be true if no password
        }
        else
        {
            EnsurePasswordIsValid(username, password);
            passwordHash = PasswordHelper.HashPassword(password);
        }

        var user = new AppUser
        {
            Username = username,
            PasswordHash = passwordHash,
            IsAdmin = isAdmin,
            IsActive = true,
            MemberId = memberId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = createdBy,
            Notes = notes,
            PasswordChangeRequired = passwordChangeRequired,
            MfaEnabled = mfaEnabled,
            PassCodeHash = null // [FIX] Explicitly ensure new users have no passcode
        };

        var newUserId = _userRepository.CreateUser(user);

        // Grant default permissions
        try
        {
            if (isAdmin)
            {
                // Admins get all permissions
                _pagePermissionRepository.GrantAllPermissions(newUserId, createdBy ?? "System");
                _auditLogger.LogAdminCreation(createdByUserId, newUserId, username, memberId);
            }
            else
            {
                // Non-admins get the system-configured default permissions
                var defaultPages = _pagePermissionRepository.GetDefaultPageIds().ToList();
                
                if (defaultPages.Any())
                {
                    _pagePermissionRepository.SetUserPermissions(newUserId, defaultPages, createdBy ?? "System");
                }
            }
        }
        catch (Exception ex)
        {
            // Log but don't fail user creation if permission granting fails
            Console.Error.WriteLine($"[UserManagement] Failed to grant default permissions for user {newUserId}: {ex.Message}");
        }

        return newUserId;
    }

    public void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true, bool mfaEnabled = false)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        if (_userRepository.UsernameExists(username, userId))
        {
            throw new InvalidOperationException($"Username '{username}' already exists.");
        }

        user.Username = username;
        var passwordChanged = false;
        if (!string.IsNullOrWhiteSpace(password))
        {
            EnsurePasswordIsValid(username, password);
            user.PasswordHash = PasswordHelper.HashPassword(password);
            user.PasswordChangeRequired = true; // Force change on next login
            passwordChanged = true;
        }
        bool statusChanged = user.IsActive != isActive;
        bool roleChanged = user.IsAdmin != isAdmin;

        user.IsAdmin = isAdmin;
        user.IsActive = isActive;
        user.MemberId = memberId;
        user.Notes = notes;
        user.MfaEnabled = mfaEnabled;

        _userRepository.UpdateUser(user);

        if (passwordChanged)
        {
            _auditLogger.LogPasswordReset(updatedByUserId, user.UserId, updatedByUserId.HasValue && updatedByUserId.Value == user.UserId);
        }

        // [FIX] Invalidate session if status changed (deactivated) or role changed
        if (statusChanged || roleChanged)
        {
            InvalidateGlobalSession(userId);
        }
    }

    // [REMOVED duplicate DeleteUser method]

    public void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        EnsurePasswordIsValid(user.Username, newPassword);

        user.PasswordHash = PasswordHelper.HashPassword(newPassword);
        if (clearPasswordChangeRequired)
        {
            user.PasswordChangeRequired = false;
        }
        _userRepository.UpdateUser(user);

        var actorUserId = performedByUserId ?? userId;
        var isSelfService = actorUserId == userId;
        _auditLogger.LogPasswordReset(actorUserId, user.UserId, isSelfService);
    }

    public void ChangePassCode(int userId, string newPassCode, bool clearPasswordChangeRequired = false, int? performedByUserId = null)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        // Basic validation for passcode (e.g. 6-8 digits)
        if (string.IsNullOrWhiteSpace(newPassCode) || !newPassCode.All(char.IsDigit) || newPassCode.Length < 6)
        {
            throw new InvalidOperationException("Passcode must be exactly 6 digits.");
        }

        // [FIX] Enforce complexity rules consistent with the user wizard
        bool isRepeating = newPassCode.All(c => c == newPassCode[0]);
        bool isSequentialInc = "0123456789".Contains(newPassCode);
        bool isSequentialDec = "9876543210".Contains(newPassCode);

        if (isRepeating || isSequentialInc || isSequentialDec)
        {
            throw new InvalidOperationException("This PIN is too easy to guess. Please choose a more complex combination.");
        }

        user.PassCodeHash = PasswordHelper.HashPassword(newPassCode);
        if (clearPasswordChangeRequired)
        {
            user.PasswordChangeRequired = false;
        }
        _userRepository.UpdateUser(user);

        // [FIX] Invalidate global in-memory cache to force security enforcement immediately
        InvalidateGlobalSession(userId);

        var actorUserId = performedByUserId ?? userId;
        var isSelfService = actorUserId == userId;
        _auditLogger.LogPasswordReset(actorUserId, user.UserId, isSelfService);
    }

    public void ClearPassCode(int userId, int? performedByUserId = null)
    {
        var user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        user.PassCodeHash = null;
        _userRepository.UpdateUser(user);

        // [FIX] Invalidate global in-memory cache to force security enforcement on next request
        InvalidateGlobalSession(userId);

        var actorUserId = performedByUserId ?? userId;
        _auditLogger.LogPasswordReset(actorUserId, user.UserId, actorUserId == userId);
    }

    public string GenerateUsernameFromMember(int memberId)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            throw new InvalidOperationException($"Member with ID {memberId} not found.");
        }

        var firstInitial = member.FirstName.Length > 0 ? member.FirstName[0].ToString().ToUpperInvariant() : "";
        var lastName = member.LastName.Replace(" ", "").Replace("-", "");
        var baseUsername = $"{firstInitial}{lastName}";

        // Ensure uniqueness
        var username = baseUsername;
        var counter = 1;
        while (_userRepository.UsernameExists(username))
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }

    public List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50)
    {
        var history = _loginHistoryRepository.GetUserLoginHistory(userId, limit);
        return history.Select(h => new LoginHistoryDto(
            h.LoginHistoryId,
            h.UserId,
            h.Username,
            h.LoginDate,
            h.IpAddress,
            h.LoginSuccessful,
            h.FailureReason)).ToList();
    }

    public List<LoginHistoryDto> GetAllLoginHistory(int limit = 100)
    {
        var history = _loginHistoryRepository.GetAllLoginHistory(limit);
        return history.Select(h => new LoginHistoryDto(
            h.LoginHistoryId,
            h.UserId,
            h.Username,
            h.LoginDate,
            h.IpAddress,
            h.LoginSuccessful,
            h.FailureReason)).ToList();
    }

    private void EnsurePasswordIsValid(string username, string password)
    {
        var result = _passwordPolicy.Validate(username, password);
        if (!result.IsValid)
        {
            var message = string.IsNullOrWhiteSpace(result.ErrorMessage)
                ? _passwordPolicy.RequirementSummary
                : result.ErrorMessage;
            throw new InvalidOperationException(message);
        }
    }

    // Page Permission Management
    public List<AppPage> GetAllPages()
    {
        return _pagePermissionRepository.GetAllPages().ToList();
    }

    public List<AppPage> GetActivePages()
    {
        return _pagePermissionRepository.GetActivePages().ToList();
    }

    public List<GFC.Core.DTOs.MobilePermissionDto> GetUserPagePermissions(int userId)
    {
        if (_userPermissionsCache.TryGetValue(userId, out var cached))
            return cached;

        var rawPermissions = _pagePermissionRepository.GetUserPermissions(userId).ToList();
        var permissions = rawPermissions.Select(p => new GFC.Core.DTOs.MobilePermissionDto
        {
            PageName = p.Page?.PageName ?? "Unknown",
            PageRoute = p.Page?.PageRoute ?? "",
            Category = p.Page?.Category,
            CanAccess = p.CanAccess,
            CanEdit = p.CanEdit
        }).ToList();

        _userPermissionsCache.TryAdd(userId, permissions);
        return permissions;
    }

    public bool UserHasPageAccess(int userId, string pageRoute)
    {
        // 1. Check if the user's permission set is already in cache
        if (!_permissionCache.TryGetValue(userId, out var routes))
        {
            // First check if user is admin (very fast check)
            var user = _userRepository.GetById(userId);
            if (user?.IsAdmin == true)
            {
                routes = new HashSet<string> { "*" }; // Admin wildcard
                _permissionCache.TryAdd(userId, routes);
                return true;
            }

            // 2. Load all permitted routes for this user into memory once
            var permissions = GetUserPagePermissions(userId);
            routes = permissions
                .Select(p => p.PageRoute.TrimStart('/').ToLowerInvariant())
                .ToHashSet();
            
            _permissionCache.TryAdd(userId, routes);
        }

        // 3. Admin wildcard check
        if (routes.Contains("*")) return true;

        // 4. Clean incoming route for comparison
        var normalized = pageRoute.TrimStart('/').ToLowerInvariant();
        
        // [MOD] Implicit access to dashboard/home for all active users
        if (string.IsNullOrEmpty(normalized) || normalized == "dashboard" || normalized == "home")
            return true;
            
        // 5. High-speed memory lookup for exact match
        if (routes.Contains(normalized))
            return true;

        // 6. Sub-route allowance (if they have access to 'members', allow 'members/114')
        return routes.Any(r => normalized.StartsWith(r + "/"));
    }

    public void SetUserPagePermissions(int userId, List<int> pageIds, string grantedBy)
    {
        _pagePermissionRepository.SetUserPermissions(userId, pageIds, grantedBy);
        // Invalidate specific user cache
        _permissionCache.TryRemove(userId, out _);
        _userPermissionsCache.TryRemove(userId, out _);
    }

    public void UpdateUserPushPreference(int userId, int pageId, bool receivePush)
    {
        _pagePermissionRepository.UpdatePushPreference(userId, pageId, receivePush);
        // Invalidate cache
        _userPermissionsCache.TryRemove(userId, out _);
    }

    public void UpdateUserEditPreference(int userId, int pageId, bool canEdit)
    {
        _pagePermissionRepository.UpdateEditPreference(userId, pageId, canEdit);
        // Invalidate cache
        _userPermissionsCache.TryRemove(userId, out _);
    }

    public GFC.Core.DTOs.MobilePermissionDto? GetUserPagePermission(int userId, string pageRoute)
    {
        var permissions = GetUserPagePermissions(userId);
        var normalized = pageRoute.TrimStart('/').ToLowerInvariant();
        
        return permissions.FirstOrDefault(p => 
            p.PageRoute.TrimStart('/').ToLowerInvariant() == normalized || 
            p.PageRoute.ToLowerInvariant() == pageRoute.ToLowerInvariant());
    }


    public void GrantAllPagePermissions(int userId, string grantedBy)
    {
        _pagePermissionRepository.GrantAllPermissions(userId, grantedBy);
        _permissionCache.TryRemove(userId, out _);
        _userPermissionsCache.TryRemove(userId, out _);
    }

    public void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy)
    {
        _pagePermissionRepository.CopyPermissions(sourceUserId, targetUserId, grantedBy);
    }

    public List<int> GetDefaultPageIds()
    {
        return _pagePermissionRepository.GetDefaultPageIds().ToList();
    }

    public void SetDefaultPageIds(List<int> pageIds)
    {
        _pagePermissionRepository.SetDefaultPageIds(pageIds);
    }
}
