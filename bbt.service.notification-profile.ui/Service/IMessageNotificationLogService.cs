
using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface IMessageNotificationLogService
    {

        [Post("/MessageNotificationLogs")]
        Task<GetMessageNotificationLogResponse> GetMessageNotificationLogs(GetMessageNotificationLogRequest logModel);

      

    }
}

