using Notification.Profile.Model.BaseResponse;
using Notification.Profile.Model.Database;

namespace Notification.Profile.Model
{
    public class GetUserRegistryModel:BaseResponseModel
    {
        public List<UserRegistry> userRegistryList { get; set; }
    }
}
