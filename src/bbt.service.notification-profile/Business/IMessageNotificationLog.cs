using Notification.Profile.Model;
using RestEase;

namespace Notification.Profile.Business
{
    public interface IMessageNotificationLog
    {
        GetMessageNotificationLogResponse GetMessageNotificationLogs([Body(BodySerializationMethod.Serialized)] GetMessageNotificationLogRequest logModel);
       
    }
}
