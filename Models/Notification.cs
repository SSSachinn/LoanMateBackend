using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    public enum NotificationType
    {
        Sent,
        Failed,
        Pending
    }

    public enum NotificationChannel
    {
        Email,
        SMS
    }

    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "Recipient user is required")]
        [ForeignKey("User")]
        public int ToUserId { get; set; }

        [Required(ErrorMessage = "Notification type is required")]
        public NotificationType Type { get; set; } = NotificationType.Pending;

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(2000, ErrorMessage = "Body cannot exceed 2000 characters")]
        public string Body { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? SentAt { get; set; }

        [Required(ErrorMessage = "Notification channel is required")]
        public NotificationChannel Status { get; set; }

        public virtual User? User { get; set; }
    }
}
