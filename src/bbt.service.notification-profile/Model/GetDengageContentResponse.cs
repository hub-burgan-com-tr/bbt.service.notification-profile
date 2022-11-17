using Notification.Profile.Model.BaseResponse;

namespace Notification.Profile.Model
{
    public class GetDengageContentResponse
    {

        public string transactionId { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public DengageGetResponse data { get; set; }
  
  
    }
    public class DengageGetResponse
    {
        public bool queryForNextPage { get; set; }
        public int affectedRowCount { get; set; }
        public int totalRowCount { get; set; }
        public List<DengageGetContents> result { get; set; }
    }

    public class DengageGetContents
    {
        public string contentName { get; set; }
        public string publicId { get; set; }
        public string updatedAt { get; set; }
        public string updatedBy { get; set; }
        public string createdAt { get; set; }
        public string createdBy { get; set; }

        public bool isTransactionalContent { get; set; }

        public string name { get; set; }

    }

}


