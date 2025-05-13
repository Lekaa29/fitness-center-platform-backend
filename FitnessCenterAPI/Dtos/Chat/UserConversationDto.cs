namespace FitnessCenterApi.Dtos.Chat;

public class UserConversationDto
{
    public int UserId { get; set; }

    public int ConversationId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}