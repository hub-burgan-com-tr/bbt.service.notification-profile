namespace Notification.Profile.Model
{
    public class GetEmailContentRequestModel
    {
        public string beginTime { get; set; }   
        public string endTime { get; set; }
        public int limit { get; set; }

        public int offset { get; set; }

    }
}
