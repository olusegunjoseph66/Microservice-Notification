namespace Notifications.Application.ViewModels.Responses
{
    public class UserNotificationItem
    {
        public int UserNotificationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string NotificationMessage { get; set; } = "";
        public bool? ReadStatus { get; set; }

    }
}