using Shared.ExternalServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class NotificationPublishMessage:IntegrationBaseMessage
    {
       // public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
    }
}
