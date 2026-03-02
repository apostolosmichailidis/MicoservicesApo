using System.ComponentModel.DataAnnotations;

namespace Apo.Services.EmailAPI_V2.Domain
{
    public class EmailRecord
    {
        [Key]
        public int EmailId { get; set; }

        [Required]
        public string RecipientEmail { get; set; } = string.Empty;

        public string? RecipientName { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public EmailStatus Status { get; set; } = EmailStatus.Pending;

        public DateTime? SentAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ErrorMessage { get; set; }
    }
}
