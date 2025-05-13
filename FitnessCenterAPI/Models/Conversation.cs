using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApi.Models;

public class Conversation
{
    [Key]
    public int IdConversation { get; set; }

    public string? Title { get; set; } // Optional for group chats
    public bool IsGroup { get; set; } = false;
    public bool isDeleted { get; set; } = false;
    public int groupOwnerId { get; set; } = -1;

    public ICollection<UserConversation> Participants { get; set; } = new List<UserConversation>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}