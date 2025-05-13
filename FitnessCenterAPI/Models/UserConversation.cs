namespace FitnessCenterApi.Models;

public class UserConversation
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ConversationId { get; set; }
    public bool isDeleted { get; set; } = false;

    public Conversation Conversation { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}