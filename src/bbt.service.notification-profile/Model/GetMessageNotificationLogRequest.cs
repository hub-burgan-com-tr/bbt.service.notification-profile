namespace Notification.Profile.Model
{
    public class GetMessageNotificationLogRequest
    {
        public long? CustomerNo { get; set; }
        public int? SourceId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
