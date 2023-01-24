using Notification.Profile.Model.BaseResponse;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Model
{
    public class GetSourceLogResponse:BaseResponseModel
    {
        public List<SourceLog> SourceLogs { get; set; }
    }
}
