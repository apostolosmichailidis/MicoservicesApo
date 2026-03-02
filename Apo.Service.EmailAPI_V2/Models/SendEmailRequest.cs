namespace Apo.Service.EmailAPI_v2.Models
{
    public class SendEmailRequest
    {
        public string RecipientEmail { get; set; } = string.Empty;
        public string? RecipientName { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
