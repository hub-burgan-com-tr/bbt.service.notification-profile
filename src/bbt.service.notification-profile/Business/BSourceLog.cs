using Notification.Profile.Enum;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Business
{
    public class BSourceLog : ISourceLog
    {
        private readonly IConfiguration _configuration;

        public BSourceLog(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public GetSourceLogResponse GetSourceLogs(GetSourceLogRequest logModel)
        {
            GetSourceLogResponse response = new GetSourceLogResponse();
            IQueryable<SourceLog> notificationLogs;

            using (var db = new DatabaseContext())
            {
                notificationLogs = (from logs in db.SourceLogs
                                    where (String.IsNullOrEmpty(logModel.Topic) || logs.Topic.Contains(logModel.Topic)) && (String.IsNullOrEmpty(logModel.PushServiceReference) || logs.PushServiceReference.Contains(logModel.PushServiceReference)) &&
                                    (String.IsNullOrEmpty(logModel.MethodType) || logs.MethodType.Contains(logModel.MethodType)) &&
                                    (String.IsNullOrEmpty(logModel.SmsServiceReference) || logs.SmsServiceReference.Contains(logModel.SmsServiceReference)) &&
                                    (String.IsNullOrEmpty(logModel.EmailServiceReference) || logs.EmailServiceReference.Contains(logModel.EmailServiceReference)) &&
                                    ((logModel.StartDate.HasValue && logModel.EndDate.HasValue) ?
                                    (logs.CreateDate >= logModel.StartDate && logs.CreateDate <= logModel.EndDate) : true)
                                    orderby logs.CreateDate descending
                                    select (logs)).Skip(((logModel.CurrentPage) - 1) * logModel.RequestItemSize)
                            .Take(logModel.RequestItemSize);
                response.Result = ResultEnum.Success;
                response.SourceLogs = notificationLogs.ToList();
                response.Count = db.MessageNotificationLogs.Count();
            }

            return response;
        }

        public SourceLogResponse PostSourceLog(SourceLogRequest data)
        {
            SourceLogResponse response = new SourceLogResponse();
            using (var db = new DatabaseContext())
            {
                SourceLog sourceLogModel = new SourceLog();
                sourceLogModel.SourceId = data.sourceLog.Id;
                sourceLogModel.Topic = data.sourceLog.Topic;
                sourceLogModel.ApiKey = data.sourceLog.ApiKey;
                sourceLogModel.Secret = data.sourceLog.Secret;
                sourceLogModel.PushServiceReference = data.sourceLog.PushServiceReference;
                sourceLogModel.SmsServiceReference = data.sourceLog.SmsServiceReference;
                sourceLogModel.EmailServiceReference = data.sourceLog.EmailServiceReference;
                sourceLogModel.KafkaUrl = data.sourceLog.KafkaUrl;
                sourceLogModel.KafkaCertificate = data.sourceLog.KafkaCertificate;
                sourceLogModel.DisplayType = data.sourceLog.DisplayType;
                sourceLogModel.Title_EN = data.sourceLog.Title_EN;
                sourceLogModel.Title_TR = data.sourceLog.Title_TR;
                sourceLogModel.ParentId = data.sourceLog.ParentId;
                sourceLogModel.ClientIdJsonPath = data.sourceLog.ClientIdJsonPath;
                sourceLogModel.RetentationTime = data.sourceLog.RetentationTime;
                sourceLogModel.ProcessName = data.sourceLog.ProcessName;
                sourceLogModel.ProcessItemId = data.sourceLog.ProcessItemId;
                sourceLogModel.ProductCodeId = data.sourceLog.ProductCodeId;
                sourceLogModel.SaveInbox = data.sourceLog.SaveInbox;
                sourceLogModel.CreateDate = DateTime.Now;
                sourceLogModel.Environment = data.Environment;
                sourceLogModel.CreateUser = data.User;
                sourceLogModel.MethodType = data.MethodType;
                sourceLogModel.InheritanceType = data.sourceLog.InheritanceType;
                sourceLogModel.AlwaysSendType = data.sourceLog.AlwaysSendType;

                db.Add(sourceLogModel);
                db.SaveChanges();
                response.sourceLog = sourceLogModel;
                response.Result = ResultEnum.Success;

                return response;
            }
        }
    }
}

