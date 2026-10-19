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
    public bool ReimbursementNotifyPush { get; set; } = false;

    // Member Signup Notifications
    public bool MemberSignupNotifyEmail { get; set; } = false;
    public bool MemberSignupNotifySMS { get; set; } = false;
    public bool MemberSignupNotifyPush { get; set; } = false;

    // Dues Payment Notifications
    public bool DuesPaymentNotifyEmail { get; set; } = false;
    public bool DuesPaymentNotifySMS { get; set; } = false;
    public bool DuesPaymentNotifyPush { get; set; } = false;

    // System Alerts
    public bool SystemAlertNotifyEmail { get; set; } = false;
    public bool SystemAlertNotifySMS { get; set; } = false;
    public bool SystemAlertNotifyPush { get; set; } = false;

    // Lottery Sales Notifications
    public bool LotterySalesNotifyEmail { get; set; } = false;
    public bool LotterySalesNotifySMS { get; set; } = false;
    public bool LotterySalesNotifyPush { get; set; } = false;

    // Controller/Access Control Events
    public bool ControllerEventNotifyEmail { get; set; } = false;
    public bool ControllerEventNotifySMS { get; set; } = false;
    public bool ControllerEventNotifyPush { get; set; } = false;

    // Granular Hall Rental Notifications
    public bool RentalNewSubmissionEmail { get; set; } = false;
    public bool RentalNewSubmissionPush { get; set; } = false;
    public bool RentalInquiryEmail { get; set; } = false;
    public bool RentalInquiryPush { get; set; } = false;
    public bool RentalPaymentEmail { get; set; } = false;
    public bool RentalPaymentPush { get; set; } = false;
    public bool RentalReplyEmail { get; set; } = false;
    public bool RentalReplyPush { get; set; } = false;

    // Reminder tracking
    public bool NotificationReminderDismissed { get; set; } = false;
    public DateTime? NotificationReminderDismissedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
