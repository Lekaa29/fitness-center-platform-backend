using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Message
{
    [Key]
    public int IdMessage { get; set; }

    public int IdSender { get; set; }
    public User Sender { get; set; }

    public int IdRecipient { get; set; }
    public User Recipient { get; set; }

    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}