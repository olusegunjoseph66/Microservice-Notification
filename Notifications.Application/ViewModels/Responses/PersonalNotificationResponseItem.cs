namespace Notifications.Application.ViewModels.Responses
{
    public class PersonalNotificationResponseItem
    {
        public int NotificationId { get; set; }

        public string Name { get; set; } = "";

        public bool Subscribed { get; set; } 
    }
}