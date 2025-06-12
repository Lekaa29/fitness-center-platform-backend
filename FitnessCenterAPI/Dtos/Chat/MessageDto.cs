namespace FitnessCenterApi.Dtos.Chat;

public class MessageDto
{
    
    public int IdMessage { get; set; }

    public int IdSender { get; set; }
    
    public string Text { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool isDeleted { get; set; } = false;
    public bool isPinned { get; set; } = false;
    
    public int IdConversation { get; set; }
}