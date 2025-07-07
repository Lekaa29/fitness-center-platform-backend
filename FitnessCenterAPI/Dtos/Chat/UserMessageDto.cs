namespace FitnessCenterApi.Dtos.Chat;

public class UserMessageDto
{
    
    public int MessageId { get; set; }
    
    public int UserId { get; set; }
    
    public bool isRead { get; set; } = false;
    
    public string Name { get; set; }
}