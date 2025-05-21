using Microsoft.AspNetCore.Mvc;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace PatientRecovery.NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationTemplatesController : ControllerBase
    {
        private readonly INotificationTemplateService _templateService;
        private readonly IMapper _mapper;

        public NotificationTemplatesController(
            INotificationTemplateService templateService,
            IMapper mapper)
        {
            _templateService = templateService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationTemplateDto>> CreateTemplate(
            [FromBody] CreateNotificationTemplateRequest request)
        {
            var template = await _templateService.CreateTemplateAsync(request);
            return CreatedAtAction(
                nameof(GetTemplate), 
                new { id = template.Id }, 
                _mapper.Map<NotificationTemplateDto>(template));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationTemplateDto>> GetTemplate(Guid id)
        {
            var template = await _templateService.GetTemplateByIdAsync(id);
            if (template == null)
                return NotFound();

            return Ok(_mapper.Map<NotificationTemplateDto>(template));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NotificationTemplateDto>> UpdateTemplate(
            Guid id,
            [FromBody] UpdateNotificationTemplateRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var template = await _templateService.UpdateTemplateAsync(request);
            if (template == null)
                return NotFound();

            return Ok(_mapper.Map<NotificationTemplateDto>(template));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(Guid id)
        {
            var success = await _templateService.DeleteTemplateAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}