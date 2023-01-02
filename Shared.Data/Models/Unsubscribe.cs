using System;
using System.Collections.Generic;

namespace Shared.Data.Models
{
    public partial class Unsubscribe
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? NotificationId { get; set; }
        public DateTime? DateUnsubscribed { get; set; }

        public virtual Notification? Notification { get; set; }
        public virtual User? User { get; set; }
    }
}
