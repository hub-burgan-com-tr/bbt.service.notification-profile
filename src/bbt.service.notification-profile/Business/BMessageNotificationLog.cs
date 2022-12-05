using Microsoft.EntityFrameworkCore;
using Notification.Profile.Enum;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Business
{
    public class BMessageNotificationLog : IMessageNotificationLog
    {
        private readonly IConfiguration _configuration;

        public BMessageNotificationLog(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public GetMessageNotificationLogResponse GetMessageNotificationLogs(GetMessageNotificationLogRequest logModel)
        {
            GetMessageNotificationLogResponse response = new GetMessageNotificationLogResponse();
            IQueryable<MessageNotificationLog> notificationLogs;
           // DateTime nullDateTime = Convert.ToDateTime("1.01.0001 00:00:00");
            using (var db = new DatabaseContext())
            {

                notificationLogs = (from logs in db.MessageNotificationLogs
                                    where logs.IsStaff==false&& (String.IsNullOrEmpty(logModel.Email) || logs.Email.Contains(logModel.Email)) && (String.IsNullOrEmpty(logModel.PhoneNumber) || logs.PhoneNumber.Contains(logModel.PhoneNumber)) &&
                                   (logModel.CustomerNo == null || logs.CustomerNo == logModel.CustomerNo) &&
                                    (String.IsNullOrEmpty(logModel.ResponseMessage) || logs.ResponseMessage.Contains(logModel.ResponseMessage)) &&
                                    //(logModel.StartDate != null || logModel.StartDate <= logs.CreateDate) && (logModel.EndDate != null || logModel.EndDate >= logs.CreateDate)
                                    ((logModel.StartDate.HasValue && logModel.EndDate.HasValue) ?
                                    (logs.CreateDate >= logModel.StartDate && logs.CreateDate <= logModel.EndDate) : true)
                                    orderby logs.CreateDate descending
                                    select (logs)).Skip(((logModel.CurrentPage) - 1) * logModel.RequestItemSize)
                            .Take(logModel.RequestItemSize); 
                response.Result = ResultEnum.Success;
                response.MessageNotificationLogs = notificationLogs.ToList();
                response.Count = db.MessageNotificationLogs.Count();
            }

            return response;
        }

    }
}
