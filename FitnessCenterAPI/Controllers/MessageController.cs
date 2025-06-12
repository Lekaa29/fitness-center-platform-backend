using System.Security.Claims;
using FitnessCenterApi.Dtos.Chat;
using FitnessCenterApi.Models;
using FitnessCenterApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace FitnessCenterApi.Controllers;
[Route("api/Conversation/")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpGet("{conversationId}/")]
    [ProducesResponseType(200, Type = typeof(List<MessageDto>))]
    public async Task<IActionResult> GetConversationMessages(int conversationId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var messages = await _messageService.GetConversationMessagesAsync(conversationId, email);
        return Ok(messages);
    }
    
    [HttpGet("chats")]
    [ProducesResponseType(200, Type = typeof(List<ConversationDto>))]
    public async Task<IActionResult> GetAllUsersConversations()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var conversations = await _messageService.GetAllUsersConversationsAsync(email);
        return Ok(conversations);
    }
    
    
    
    [HttpGet("{conversationId}/participants")]
    [ProducesResponseType(200, Type = typeof(List<UserDto>))]
    public async Task<IActionResult> GetAllConversationParticipants(int conversationId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var users = await _messageService.GetAllConversationParticipantsAsync(conversationId, email);
        return Ok(users);
    }
    
    [HttpGet("{conversationId}/search/")]
    [ProducesResponseType(200, Type = typeof(List<MessageDto>))]
    public async Task<IActionResult> SearchConversationMessages(int conversationId, string searchQuery)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var messages = await _messageService.SearchConversationMessages(conversationId, searchQuery, email);
        return Ok(messages);
    }
    
    [HttpPost("message/send/{recipientId}")]
    public async Task<IActionResult> AddMessage([FromBody] MessageDto messageDto, int recipientId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (messageDto == null)
        {
            return BadRequest("Message object is null");
        }
        var result = await _messageService.AddMessageAsync(messageDto,recipientId, email);
        if (result)
        {
            return Ok("Message added successfully");
        }
        return BadRequest("Message not added");
    }   
    
    [HttpPost("message/markAsRead")]
    public async Task<IActionResult> MarkMessagesAsRead([FromBody] IdListDto idListDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (idListDto == null)
        {
            return BadRequest("IdList object is null");
        }
        var result = await _messageService.MarkMessagesAsReadAsync(idListDto, email);
        if (result)
        {
            return Ok("Messages marked as read added successfully");
        }
        return BadRequest("Messages marked as read unsuccessfully");
    } 
    
    
    [HttpDelete("message/remove/{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
    
        var result = await _messageService.DeleteMessageAsync(messageId, email);
        if (result)
        {
            return Ok("Messages deleted successfully");
        }
        return BadRequest("Messages deleted unsuccessfully");
    } 
    [HttpDelete("remove/{conversationId}/")]
    public async Task<IActionResult> DeleteConversation(int conversationId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
    
        var result = await _messageService.DeleteConversationAsync(conversationId, email);
        if (result)
        {
            return Ok("Conversation deleted successfully");
        }
        return BadRequest("Conversation deleted unsuccessfully");
    } 
    
    
    [HttpPut("message/update/{messageId}/")]
    public async Task<IActionResult> EditMessage([FromRoute] int messageId, [FromBody] MessageDto messageDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (messageDto == null)
        {
            return BadRequest("Message object is null");
        }
        var result = await _messageService.EditMessageAsync(messageId, messageDto, email);
        if (result)
        {
            return Ok("Message updated successfully");
        }
        return BadRequest("Message not updated");
    }  
    
    [HttpGet("{conversationId}/message/unreadCount")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> GetConversationUnreadMessages(int conversationId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var count = await _messageService.GetConversationUnreadMessagesAsync(conversationId, email);
        return Ok(count);
    }
    
    [HttpGet("unreadCount")]
    [ProducesResponseType(200, Type = typeof(int))]
    public async Task<IActionResult> GetTotalUnreadMessages()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }

        var count = await _messageService.GetTotalUnreadMessagesAsync(email);
        return Ok(count);
    }
    
    [HttpPost("createGroup")]
    public async Task<IActionResult> CreateGroupConversation([FromBody] GroupDto groupDto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        if (groupDto == null)
        {
            return BadRequest("Group object is null");
        }
        var result = await _messageService.AddGroupConversationAsync(groupDto, email);
        if (result)
        {
            return Ok("Group created successfully");
        }
        return BadRequest("Group not created");
    } 
    
    [HttpPost("{conversationId}/addParticipant")]
    public async Task<IActionResult> AddGroupParticipant([FromQuery] int conversationId, [FromBody] int newParticipantId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        var result = await _messageService.AddGroupParticipantAsync(conversationId, newParticipantId, email);
        if (result)
        {
            return Ok("Member added successfully");
        }
        return BadRequest("Member not added");
    } 
    
    [HttpPost("{conversationId}/removeParticipant")]
    public async Task<IActionResult> RemoveGroupParticipant([FromQuery] int conversationId, [FromBody] int newParticipantId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        var result = await _messageService.RemoveGroupParticipantAsync(conversationId, newParticipantId, email);
        if (result)
        {
            return Ok("Member removed successfully");
        }
        return BadRequest("Member not removed");
    }

    [HttpPost("{conversationId}/leaveGroup")]
    public async Task<IActionResult> LeaveGroup([FromQuery] int conversationId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        var result = await _messageService.LeaveGroupAsync(conversationId, email);
        if (result)
        {
            return Ok("Left group successfully");
        }
        return BadRequest("Left group unsuccessfully");
    }
    
    [HttpPost("{messageId}/pinMessage")]
    public async Task<IActionResult> PinMessage([FromQuery] int messageId)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
     
        if (email == null)
        {
            return Unauthorized("Invalid attempt");
        }
        var result = await _messageService.PinMessageAsync(messageId, email);
        if (result)
        {
            return Ok("Pinned message successfully");
        }
        return BadRequest("Pinned message unsuccessfully");
    }
    
       
}

