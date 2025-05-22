using Microsoft.AspNetCore.Mvc;
using NotificationService.Services;
using NotificationService.DTOs;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet("history/{userId1}/{userId2}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatHistory(
            string userId1, 
            string userId2,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50)
        {
            try
            {
                var messages = await _chatService.GetChatHistoryAsync(userId1, userId2, skip, take);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history");
                return StatusCode(500, "An error occurred while getting chat history");
            }
        }

        [HttpGet("unread/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetUnreadMessages(string userId)
        {
            try
            {
                var messages = await _chatService.GetUnreadMessagesAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread messages");
                return StatusCode(500, "An error occurred while getting unread messages");
            }
        }

        [HttpGet("unread/count/{userId}")]
        public async Task<ActionResult<int>> GetUnreadMessageCount(string userId)
        {
            try
            {
                var count = await _chatService.GetUnreadMessageCountAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread message count");
                return StatusCode(500, "An error occurred while getting unread message count");
            }
        }

        [HttpPost("mark-as-read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(Guid messageId)
        {
            try
            {
                await _chatService.MarkMessageAsReadAsync(messageId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read");
                return StatusCode(500, "An error occurred while marking message as read");
            }
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var result = await _chatService.DeleteMessageAsync(messageId, userId);
                if (!result)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message");
                return StatusCode(500, "An error occurred while deleting message");
            }
        }
    }
}