using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;

namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [HttpPost("/notification/setting")]

        public async Task<ActionResult<NotificationSaveResponse>> SaveNotificationSettings(IEnumerable<NoticationSettingsSaveRequest> noticationSettings, CancellationToken cancellationToken = default)
        {
            var response=new NotificationSaveResponse();


            response = await _notificationsService.SaveNotificationSettings(noticationSettings, cancellationToken);

            if(response.Status.ToLower().Contains("failed"))
            {
                return BadRequest(response);
            }
            else
            {
                return response;
            }

           
        }

        [HttpGet("/notification")]

        public async Task<ActionResult<MyNotificationSettingsResponse>> GetMyNotificationSettings(CancellationToken cancellationToken = default)
        {



            var response=await _notificationsService.GetMyNotificationSettings( cancellationToken);

            if(response.Status.ToLower().Contains("failed"))
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("/notification/user")]

        public async Task<ActionResult<MyNotificationsResponse>> GetMyNotifications([FromQuery] NotificationQueryFilter filter, CancellationToken cancellationToken = default)
        {
            var response = new MyNotificationsResponse();


             response = await _notificationsService.GetMyNotifications(filter,cancellationToken);

            if (response.Status.ToLower().Contains("failed"))
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }


        [HttpGet("/notification/user/acknowledge")]

        public async Task<ActionResult<AcknowledgeNotificationResponse>> AcknowledgeNotification([FromBody] AcknowledgeNotificationInput input, CancellationToken cancellationToken = default)
        {
            var response = new AcknowledgeNotificationResponse();


            response = await _notificationsService.AcknowledgeNotification(input, cancellationToken);

            if (response.Status.ToLower().Contains("failed"))
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

    }
}
