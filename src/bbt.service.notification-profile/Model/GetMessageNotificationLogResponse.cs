using Notification.Profile.Model.BaseResponse;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Model
{
    public class GetMessageNotificationLogResponse: BaseResponseModel
    {
        public List<MessageNotificationLog> MessageNotificationLogs { get; set; }
    }
}
