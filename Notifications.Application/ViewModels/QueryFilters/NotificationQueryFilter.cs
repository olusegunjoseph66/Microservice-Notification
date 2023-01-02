using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.ViewModels.QueryFilters
{
    public class NotificationQueryFilter
    {

        public bool? ReadStatus { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
