namespace Notifications.Application.ViewModels.Responses
{
    public class PersonalNotificationResponse
    {

        public List<PersonalNotificationResponseItem> NotificationSettings { get; set; } = new List<PersonalNotificationResponseItem>();

    }
}