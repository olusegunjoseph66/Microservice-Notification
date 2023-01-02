using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.ViewModels.Requests
{
    public class NoticationSettingsSaveRequest
    {
        public bool Subscribed { get; set; }
        public int? NotificationId { get; set; }
    }
}
