using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.ViewModels.Responses
{
    public class AcknowledgeNotificationResponse
    {
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";

        public NotificationResponseData Data { get; set; } = new NotificationResponseData();
    }
}
