using Microsoft.Extensions.Azure;
using Notifications.Application.DTOs.Filters;
using Shared.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Infrastructure.QueryObjects
{
    public class NotificationQueryObject : QueryObject<Shared.Data.Models.UserNotification>
    {
        public NotificationQueryObject(NotificationFilterDto filter)
        {
            if (filter == null) return;

            // if (!string.IsNullOrWhiteSpace(filter.ReadStatus))
            //  And(p => p.RequestStatus.Code == filter.RequestStatusCode);

            And(p => p.ReadStatus == filter.ReadStatus);

           


        }
    }
}
