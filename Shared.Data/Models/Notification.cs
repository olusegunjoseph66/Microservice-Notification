using System;
using System.Collections.Generic;

namespace Shared.Data.Models
{
    public partial class Notification
    {
        public Notification()
        {
            Unsubscribes = new HashSet<Unsubscribe>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool OptionalNotification { get; set; }
        public bool? IsDistributorNotification { get; set; }
        public string? EventTriggerName { get; set; }
        public string? EmailTemplateId { get; set; }
        public string? SmsMessageTemplate { get; set; }
        public string? PushMessageTemplate { get; set; }

        public virtual ICollection<Unsubscribe> Unsubscribes { get; set; }
    }
}
