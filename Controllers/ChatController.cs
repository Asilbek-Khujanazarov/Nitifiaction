using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet("history/{otherUserId}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatHistory(string otherUserId)
        {
            var currentUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var messages = await _chatService.GetChatHistoryAsync(currentUserId, otherUserId);
            return Ok(messages);
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetUnreadMessages()
        {
            var currentUserId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var messages = await _chatService.GetUnreadMessagesAsync(currentUserId);
            return Ok(messages);
        }

        [HttpPost("mark-as-read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(Guid messageId)
        {
            await _chatService.MarkMessageAsReadAsync(messageId);
            return Ok();
        }
    }
}