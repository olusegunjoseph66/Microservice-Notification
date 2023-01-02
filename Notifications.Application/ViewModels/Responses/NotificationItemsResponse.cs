namespace Notifications.Application.ViewModels.Responses
{
    public class NotificationItemsResponse
    {
        public NotificationItemsResponse()
        {
        }
        public Pagination Pagination { get; set; } = new Pagination();
        public List<UserNotificationItem> UserNotifications { get; set; } = new List<UserNotificationItem>();
        
    }
}