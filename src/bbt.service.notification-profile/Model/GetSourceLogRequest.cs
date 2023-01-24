using Notification.Profile.Model.BaseSearchRequest;

namespace Notification.Profile.Model
{
    public class GetSourceLogRequest:BaseSearchRequestModel
    {
        public string Topic { get; set; }
        public string PushServiceReference { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MethodType { get; set; }


    }
}
