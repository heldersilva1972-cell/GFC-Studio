using System;

namespace GFC.Core.DTOs
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; }
        public int? MemberId { get; set; }
        public string? Notes { get; set; }
        public bool PasswordChangeRequired { get; set; }
        public bool MfaEnabled { get; set; }
    }

    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public int? MemberId { get; set; }
        public string? Notes { get; set; }
        public bool MfaEnabled { get; set; }
    }

    public class UserPermissionUpdateRequest
    {
        public int UserId { get; set; }
        public List<int> PageIds { get; set; } = new();
    }
}
