using Notification.Profile.Model.BaseResponse;

namespace Notification.Profile.Model
{
    public class GetInstantDGReminderResponse:BaseResponseModel
    {
        public long CUSTOMER_NUMBER { get; set; }
        public string PRODUCT_CODE { get; set; }
        public bool SEND_SMS { get; set; }
        public bool SEND_EMAIL { get; set; }
        public bool SEND_PUSHNOTIFICATION { get; set; }
        public bool SEND_NOTIFICATION { get; set; }

    }
}
