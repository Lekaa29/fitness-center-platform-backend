using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Notification
{
    [Key]
    public int IdNotification { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}