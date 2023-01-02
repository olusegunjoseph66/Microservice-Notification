using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Interfaces.Services
{
    public interface INotificationsService
    {
        Task<NotificationSaveResponse> SaveNotificationSettings(IEnumerable<NoticationSettingsSaveRequest> noticationSettings, CancellationToken cancellationToken);
        Task<MyNotificationSettingsResponse> GetMyNotificationSettings(CancellationToken cancellationToken);
        Task<MyNotificationsResponse> GetMyNotifications(NotificationQueryFilter filter, CancellationToken cancellationToken);
        Task<AcknowledgeNotificationResponse> AcknowledgeNotification(AcknowledgeNotificationInput input, CancellationToken cancellationToken);
    }
}
