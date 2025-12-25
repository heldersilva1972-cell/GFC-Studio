// [NEW]
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public AnnouncementsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SystemNotification>>> Get()
        {
            var announcements = await _notificationService.GetActiveNotificationsAsync();
            return Ok(announcements);
        }
    }
}
