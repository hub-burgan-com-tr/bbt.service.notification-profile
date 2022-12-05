namespace Notification.Profile.Model.BaseSearchRequest
{
    public class BaseSearchRequestModel
    {
        public int CurrentPage { get; set; }
        public int RequestItemSize { get; set; }
        public string SortProperty { get; set; }
        public bool SortReverse { get; set; }

        public BaseSearchRequestModel() : base()
        {
            CurrentPage = 1;
            RequestItemSize = 10;
        }
    }
}
