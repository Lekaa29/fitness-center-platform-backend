namespace FitnessCenterApi.Dtos.Chat;

public class ConversationDto
{
    public int IdConversation { get; set; }
    public string? Title { get; set; } // Optional for group chats
    public bool IsGroup { get; set; } = false;
    public bool isDeleted { get; set; } = false;
    public int groupOwnerId { get; set; } = -1;
}