using Notification.Profile.Model.BaseResponse;
namespace Notification.Profile.Model
{
    public class SourceListResponse:BaseResponseModel
    {
        public List<Notification.Profile.Model.Database.Source> sources { get; set; }
    }
}
