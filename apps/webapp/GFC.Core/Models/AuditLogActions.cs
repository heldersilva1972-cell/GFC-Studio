// [NEW]
namespace GFC.Core.Models
{
    public static class AuditLogActions
    {
        // Authentication
        public const string LoginSuccess = "LoginSuccess";
        public const string LoginSuccessMagicLink = "LoginSuccessMagicLink";
        public const string LoginFailed = "LoginFailed";
        public const string Logout = "Logout";
        public const string MagicLinkSent = "MagicLinkSent";
        public const string LogoutIdle = "LogoutIdle";
        public const string LogoutAbsolute = "LogoutAbsolute";
        public const string SessionInvalidatedVpnLost = "SessionInvalidatedVpnLost";

        // User Management
        public const string UserCreated = "UserCreated";
        public const string UserUpdated = "UserUpdated";
        public const string UserDeleted = "UserDeleted";
        public const string PasswordChanged = "PasswordChanged";
    }
}
