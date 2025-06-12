using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApi.Repositories;

public class MessageRepository
{
    private readonly FitnessCenterDbContext _context;
    
    public MessageRepository(FitnessCenterDbContext context)
    {
        _context = context;
    }
    
    public async  Task<ICollection<Message>> GetConversationMessagesAsync(int conversationId, int userId)
    {   

        var messages = await _context.Messages.Where(
            m => m.IdConversation == conversationId).ToListAsync();
      
        return messages;
    }
    
    public async  Task<ICollection<Conversation>> GetAllUsersConversationsAsync(int userId)
    {   
        
        var userConversations = await _context.UserConversations
            .Where(uc => uc.UserId == userId)
            .ToListAsync();

        var conversationIds = userConversations.Select(uc => uc.ConversationId).ToList();

        var conversations = await _context.Conversations
            .Where(c => conversationIds.Contains(c.IdConversation))
            .ToListAsync();

        return conversations;
    }
    
    
    
    public async  Task<ICollection<User>> GetAllConversationParticipantsAsync(int conversationId)
    {   

        var userConversations = await _context.UserConversations.Where(
            u => u.ConversationId == conversationId).ToListAsync();
        
        var userIds = userConversations.Select(uc => uc.UserId).ToList();
        
        var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        
        return users;
    }
    
    public async  Task<UserConversation?> GetUserConversationByConversationAsync(int conversationId, int userId)
    {   

        var userConversations = await _context.UserConversations
            .Where(u => u.ConversationId == conversationId && u.UserId == userId)
            .FirstOrDefaultAsync();

        return userConversations;
    }
    
   
    public async  Task<Message?> GetMessageAsync(int messageId)
    {

        var message = await _context.Messages.Where(
            m => m.IdMessage == messageId).FirstOrDefaultAsync();

        return message;
    }
    
    public async  Task<UserMessage?> GetUserMessageAsync(int messageId, int userId)
    {

        var userMessage = await _context.UserMessages.Where(
            u => u.MessageId == messageId && u.UserId == userId).FirstOrDefaultAsync();
    
        return userMessage;
    }
    
    public async  Task<int> GetConversationUnreadMessagesCountAsync(int conversationId, int userId)
    {

        var unreadCount = await _context.UserMessages
            .Where(um => um.UserId == userId &&
                         um.Message.IdConversation == conversationId &&
                         !um.isRead)
            .CountAsync();

        return unreadCount;
    }
    
    public async  Task<int> GetTotalUnreadMessagesAsync(int userId)
    {
        
        var totalCount = await _context.UserMessages
            .Where(um => um.UserId == userId && !um.isRead)
            .CountAsync();

        return totalCount;
    }
    
    public async  Task<Conversation?> GetConversationAsync(int conversationId)
    {

        var conversation = await _context.Conversations.Where(
            c => c.IdConversation == conversationId).FirstOrDefaultAsync();

        return conversation;
    }
        
    public async  Task<ICollection<Message>> SearchConversationMessagesAsync(int conversationId, string searchQuery, int userId)
    {   

        var messages = await _context.Messages
            .Where(m => m.IdConversation == conversationId && m.Text.Contains(searchQuery))
            .ToListAsync();

        return messages;
    }
    
    public async Task<int> AddMessageAsync(Message message) 
    {
        await _context.Messages.AddAsync(message);
        bool status = await SaveAsync();
        if (status == false) return -1;

        return message.IdMessage;

    }
    
    public async Task<bool> AddUserMessageAsync(UserMessage userMessage) 
    {
        await _context.UserMessages.AddAsync(userMessage);
        return await SaveAsync();

    }
    
    public async Task<int> AddConversationAsync(Conversation conversation) 
    {
        await _context.Conversations.AddAsync(conversation);
        bool status = await SaveAsync();
        if (status == false) return -1;

        return conversation.IdConversation;

    }
    
    public async Task<bool> AddUserConversationAsync(UserConversation userConversation) 
    {
        await _context.UserConversations.AddAsync(userConversation);
        return await SaveAsync();

    }
    
    public async Task<bool> UpdateUserMessagesAsync(List<UserMessage> userMessages) 
    {
        foreach (var item in userMessages)
        {
            _context.UserMessages.Update(item);
        }
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateMessagesAsync(List<Message> message) 
    {
        foreach (var item in message)
        {
            _context.Messages.Update(item);
        }
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateMessageAsync(Message message) 
    {
    
        _context.Messages.Update(message);
        
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateConversationAsync(Conversation conversation) 
    {
    
        _context.Conversations.Update(conversation);
        
        return await SaveAsync();
    }
    
    public async Task<bool> UpdateUserConversationAsync(UserConversation userConversation) 
    {
    
        _context.UserConversations.Update(userConversation);
        
        return await SaveAsync();
    }
    
    
    
    private async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}