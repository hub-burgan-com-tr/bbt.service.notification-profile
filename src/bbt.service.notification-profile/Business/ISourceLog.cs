using Notification.Profile.Model;
using RestEase;

namespace Notification.Profile.Business
{
    public interface ISourceLog
    {
        GetSourceLogResponse GetSourceLogs([Body(BodySerializationMethod.Serialized)] GetSourceLogRequest logModel);
        SourceLogResponse PostSourceLog([Body(BodySerializationMethod.Serialized)] SourceLogRequest logModel);
    }
}
