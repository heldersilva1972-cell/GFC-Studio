using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("UserNotificationPreferences")]
public class UserNotificationPreferences
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    // Reimbursement Notifications
    public bool ReimbursementNotifyEmail { get; set; } = false;
    public bool ReimbursementNotifySMS { get; set; } = false;

    // Member Signup Notifications
    public bool MemberSignupNotifyEmail { get; set; } = false;
    public bool MemberSignupNotifySMS { get; set; } = false;

    // Dues Payment Notifications
    public bool DuesPaymentNotifyEmail { get; set; } = false;
    public bool DuesPaymentNotifySMS { get; set; } = false;

    // System Alerts
    public bool SystemAlertNotifyEmail { get; set; } = false;
    public bool SystemAlertNotifySMS { get; set; } = false;

    // Lottery Sales Notifications
    public bool LotterySalesNotifyEmail { get; set; } = false;
    public bool LotterySalesNotifySMS { get; set; } = false;

    // Controller/Access Control Events
    public bool ControllerEventNotifyEmail { get; set; } = false;
    public bool ControllerEventNotifySMS { get; set; } = false;

    // Reminder tracking
    public bool NotificationReminderDismissed { get; set; } = false;
    public DateTime? NotificationReminderDismissedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
