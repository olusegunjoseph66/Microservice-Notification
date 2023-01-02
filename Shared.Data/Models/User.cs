using System;
using System.Collections.Generic;

namespace Shared.Data.Models
{
    public partial class User
    {
        public User()
        {
            Unsubscribes = new HashSet<Unsubscribe>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DeviceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public string? Roles { get; set; }

        public virtual ICollection<Unsubscribe> Unsubscribes { get; set; }
    }
}
