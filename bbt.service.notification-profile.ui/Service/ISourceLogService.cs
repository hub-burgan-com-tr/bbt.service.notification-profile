
using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface ISourceLogService
    {

        [Post("/SourceLogs")]
        Task<GetSourceLogResponse> GetSourceLogs(GetSourceLogRequest logModel);

      

    }
}

