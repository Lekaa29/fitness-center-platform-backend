using AutoMapper;
using FitnessCenterApi.Dtos;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;

namespace FitnessCenterApi.Services;

public class MessageService
{
    private readonly IMapper _mapper;
    private readonly UserRepository _userRepository;
    private readonly MessageRepository _messageRepository;
    private readonly IConfiguration _configuration;


    public MessageService(IMapper mapper, IConfiguration configuration, UserRepository userRepository, MessageRepository messageRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _configuration = configuration;
    }
    
    
    public async Task<ICollection<MessageDto>?> GetConversationMessagesAsync(int conversationId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var messages = await _messageRepository.GetConversationMessagesAsync(conversationId, user.Id);

        var messagesDtos = _mapper.Map<ICollection<MessageDto>>(messages);
        
        return messagesDtos;
    }
    public async Task<int?> GetConversationIdByRecepient(int recepientId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return -1;
        }
        var conversationId = await _messageRepository.GetConversationIdByRecipient(recepientId, user.Id);


        if (conversationId == null) return -1;
        return conversationId;
    }
    

    public async Task<ICollection<ConversationDto>?> GetAllUsersConversationsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var conversations = await _messageRepository.GetAllUsersConversationsAsync(user.Id);

        var conversationsDtos = _mapper.Map<ICollection<ConversationDto>>(conversations);
        
        return conversationsDtos;
    }
    
 
    
    public async Task<ICollection<UserDto>?> GetAllConversationParticipantsAsync(int conversationId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        
        var users = await _messageRepository.GetAllConversationParticipantsAsync(conversationId);

        var userDtos = _mapper.Map<ICollection<UserDto>>(users);
        
        return userDtos;
    }
    
    public async Task<ICollection<MessageDto>?> SearchConversationMessages(int conversationId, string searchQuery, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        var userConversation = _messageRepository.GetUserConversationByConversationAsync(conversationId, user.Id);
        if (userConversation == null) return new List<MessageDto>();
        
        
        var messages = await _messageRepository.SearchConversationMessagesAsync(conversationId, searchQuery, user.Id);

        var messagesDtos = _mapper.Map<ICollection<MessageDto>>(messages);
        
        return messagesDtos;
    }
    
    
    
    public async Task<bool> AddMessageAsync(MessageDto messageDto, int recipientId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        var message = _mapper.Map<Message>(messageDto);
        
        
        message.Sender = user;
        message.Timestamp = DateTime.Now;
        
        User? recipient = null;
        
        if (recipientId != -1)
        {
            //generate new chat for first messages
            
            recipient = await _userRepository.GetUserAsync(recipientId);
            if (recipient == null) return false;
            Conversation newConversation = new Conversation();
            int conversationId = await _messageRepository.AddConversationAsync(newConversation);
            
            //update message to be connected to the new chat
            message.Conversation = newConversation;
            message.IdConversation = conversationId;
            
            //generate both connections to the conversation
            UserConversation userConversation = new UserConversation();
            userConversation.ConversationId = conversationId;
            userConversation.UserId = recipient.Id;
            userConversation.User = recipient;
            bool status = await _messageRepository.AddUserConversationAsync(userConversation);
            if (status == false) return false;
            userConversation = new UserConversation();
            userConversation.ConversationId = conversationId;
            userConversation.UserId = user.Id;
            userConversation.User = user;
            status = await _messageRepository.AddUserConversationAsync(userConversation);
            if (status == false) return false;
            
        }
        else
        {
            Conversation existingConversation = await _messageRepository.GetConversationAsync(messageDto.IdConversation);
            if (existingConversation == null) return false;
            
            message.Conversation = existingConversation;
            message.IdConversation = existingConversation.IdConversation;
        }
        
        int messageId = await _messageRepository.AddMessageAsync(message);
        
        if (messageId < 0) return false;

        if (recipientId != -1)
        {
            UserMessage newUserMessage = new UserMessage();
            newUserMessage.UserId = recipientId;
            newUserMessage.User = recipient;
            newUserMessage.Message = message;
            newUserMessage.MessageId = messageId;
            newUserMessage.isRead = false;

            bool status = await _messageRepository.AddUserMessageAsync(newUserMessage);
            if (status == false) return false;
            
            newUserMessage = new UserMessage();
            newUserMessage.UserId = user.Id;
            newUserMessage.User = user;
            newUserMessage.Message = message;
            newUserMessage.MessageId = messageId;
            newUserMessage.isRead = true;
            
            
            return await _messageRepository.AddUserMessageAsync(newUserMessage);

        }
        else
        {
            
            var recepients = await _messageRepository.GetAllConversationParticipantsAsync(messageDto.IdConversation);
            foreach (var recepient in recepients)
            {
                UserMessage newUserMessage = new UserMessage();
                newUserMessage.UserId = recepient.Id;
                newUserMessage.User = recepient;
                newUserMessage.Message = message;
                newUserMessage.MessageId = messageId;
                
                bool status = await _messageRepository.AddUserMessageAsync(newUserMessage);
                if (status == false) return false;
                
            }
        }
        return true;
    }
    
    
    
    
    
    public async Task<bool> MarkMessagesAsReadAsync(IdListDto idListDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        List<UserMessage> messages = new List<UserMessage>();

        foreach (int messageId in idListDto.Ids)
        {

            UserMessage newMessage = await _messageRepository.GetUserMessageAsync(messageId, user.Id);
            if (newMessage == null)
            {
                return false;
            }

            newMessage.isRead = true;
            messages.Add(newMessage);
        }

        //all messages are here checked to exist in our db
        //we can continue marking them as read

        return await _messageRepository.UpdateUserMessagesAsync(messages);
        
    }
    
    public async Task<bool> DeleteMessageAsync(int messageId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        
        Message message = await _messageRepository.GetMessageAsync(messageId);
        if (message.IdSender != user.Id)
        {
            //you can delete only your messages
            return false;
        }
        message.isDeleted = true;

        return await _messageRepository.UpdateMessageAsync(message);
    }
    
    public async Task<bool> DeleteConversationAsync(int conversationId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        
        Conversation conversation = await _messageRepository.GetConversationAsync(conversationId);
        if (conversation.groupOwnerId != user.Id)
        {
            //only admin can delete group chat
            return false;
        }

        conversation.isDeleted = true;

        return await _messageRepository.UpdateConversationAsync(conversation);
    }
    
    public async Task<bool> EditMessageAsync(int messageId, MessageDto messageDto, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        Message oldMessage = await _messageRepository.GetMessageAsync(messageId);
        if (oldMessage.IdSender != user.Id)
        {
            // you can edit only your messages
            return false;
        }
        
        var message = _mapper.Map<Message>(messageDto);
        
        message.Sender = user;
        message.Timestamp = DateTime.Now;

        return await _messageRepository.UpdateMessageAsync(message);
    }
    
    public async Task<int?> GetConversationUnreadMessagesAsync(int conversationId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        int unreadCount = await _messageRepository.GetConversationUnreadMessagesCountAsync(conversationId, user.Id);
        
        return unreadCount;
    }
    public async Task<int?> GetTotalUnreadMessagesAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        int unreadCount = await _messageRepository.GetTotalUnreadMessagesAsync(user.Id);
        
        return unreadCount;
    }
    
    public async Task<bool> AddGroupConversationAsync(GroupDto groupDto, string email)
    {
        
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        
        List<User> participants = new List<User>();
        
        foreach (string userId in groupDto.ParticipentIds)
        {
            User newParticipant = await _userRepository.GetUserAsync(int.Parse(userId));
            if (newParticipant == null || newParticipant.Status == 0)
            {
                //if not existing or deleted
                return false;
            }
            participants.Add(newParticipant);    
        }

        

        Conversation newConversation = new Conversation();
        newConversation.Title = groupDto.GroupName;
        newConversation.groupOwnerId = user.Id;
        newConversation.IsGroup = true;

        int newConversationId = await _messageRepository.AddConversationAsync(newConversation);

        
        
        foreach (User participant in participants)
        {
            UserConversation newUserConversation = new UserConversation();
            newUserConversation.User = participant;
            newUserConversation.ConversationId = newConversationId;
            newUserConversation.UserId = participant.Id;
            newUserConversation.Conversation = newConversation;

            bool newAddedUserConversation = await _messageRepository.AddUserConversationAsync(newUserConversation);
            if (newAddedUserConversation == false)
            {
                return false;
            }
        }

        return true;
    }


    public async Task<bool> AddGroupParticipantAsync(int conversationId, int newParticipantId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        User newMember = await _userRepository.GetUserAsync(newParticipantId);
        if (newMember == null) return false;

        Conversation existingConversation = await _messageRepository.GetConversationAsync(conversationId);
        if (existingConversation == null) return false;

        UserConversation newUserConversation = new UserConversation();
        newUserConversation.User = newMember;
        newUserConversation.ConversationId = conversationId;
        newUserConversation.UserId = newParticipantId;
        newUserConversation.Conversation = existingConversation;

        return await _messageRepository.AddUserConversationAsync(newUserConversation);
        
    }
    
    public async Task<bool> RemoveGroupParticipantAsync(int conversationId, int ParticipantId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        User existingMember = await _userRepository.GetUserAsync(ParticipantId);
        if (existingMember == null) return false;

        Conversation existingConversation = await _messageRepository.GetConversationAsync(conversationId);
        if (existingConversation == null) return false;

        UserConversation existingUserConversation = await _messageRepository.GetUserConversationByConversationAsync(conversationId, ParticipantId);
        if (existingUserConversation == null) return false;


        existingUserConversation.isDeleted = true;
        

        return await _messageRepository.UpdateUserConversationAsync(existingUserConversation);
        
    }
    
    public async Task<bool> LeaveGroupAsync(int conversationId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        
        Conversation existingConversation = await _messageRepository.GetConversationAsync(conversationId);
        if (existingConversation == null) return false;

        UserConversation existingUserConversation = await _messageRepository.GetUserConversationByConversationAsync(conversationId, user.Id);
        if (existingUserConversation == null) return false;


        existingUserConversation.isDeleted = true;
        
        return await _messageRepository.UpdateUserConversationAsync(existingUserConversation);
        
    }
    
    public async Task<bool> PinMessageAsync(int messageId, string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        Message message = await _messageRepository.GetMessageAsync(messageId);
        message.isPinned = true;
        
        return await _messageRepository.UpdateMessageAsync(message);
        
    }

}