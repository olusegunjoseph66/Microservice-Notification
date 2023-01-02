using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data.Models
{
    public partial class UserNotification
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string NotificationMessage { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateRead { get; set; }
        public bool? ReadStatus { get; set; }

        public virtual Notification Notification { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
