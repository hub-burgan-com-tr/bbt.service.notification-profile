using Notification.Profile.Model.BaseResponse;

namespace Notification.Profile.Model
{
    public class GetTemplateResponseModel : BaseResponseModel
    {
     public   List<ContentInfo> ContentList { get; set; }
    }
}

