namespace FitnessCenterApi.Models;

public class UserMessage
{
    public int MessageId { get; set; }
    public Message Message { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public bool isRead { get; set; } = false;
    
}