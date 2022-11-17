using Notification.Profile.Model.BaseResponse;
using Notification.Profile.Model.Database;
namespace Notification.Profile.Model
{
    public class GetProductCodeResponse:BaseResponseModel
    {
      
            public List<ProductCode> ProductCodes { get; set; }
        
    }
}
