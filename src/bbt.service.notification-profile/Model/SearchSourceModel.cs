using Notification.Profile.Model.BaseSearchRequest;

namespace Notification.Profile.Model
{
    public class SearchSourceModel : BaseSearchRequestModel
    {
        public string Topic { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }
        public string Title { get; set; }
    }

    public class GetSourceModel 
    {
        public string Topic { get; set; }
        public string Title { get; set; }
    }
}
