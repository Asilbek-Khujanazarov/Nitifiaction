using Microsoft.AspNetCore.Mvc;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;
using AutoMapper;

namespace PatientRecoverySystem.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;
        private readonly IMapper _mapper;

        public NotificationsController(
            INotificationService notificationService,
            ILogger<NotificationsController> logger,
            IMapper mapper)
        {
            _notificationService = notificationService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            try
            {
                var notification = await _notificationService.CreateNotificationAsync(request);
                return CreatedAtAction(
                    nameof(GetNotification), 
                    new { id = notification.Id }, 
                    _mapper.Map<NotificationDto>(notification));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification");
                return StatusCode(500, "An error occurred while creating the notification");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetNotification(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound();

            return Ok(_mapper.Map<NotificationDto>(notification));
        }

        [HttpGet("recipient/{recipientId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetRecipientNotifications(
            string recipientId,
            [FromQuery] NotificationStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var notifications = await _notificationService.GetNotificationsByRecipientAsync(
                recipientId,
                status,
                fromDate,
                page,
                pageSize);

            return Ok(_mapper.Map<IEnumerable<NotificationDto>>(notifications));
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<BulkNotificationResultDto>> SendBulkNotifications(
            [FromBody] BulkNotificationRequest request)
        {
            try
            {
                var result = await _notificationService.SendBulkNotificationsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notifications");
                return StatusCode(500, "An error occurred while sending bulk notifications");
            }
        }
    }
}