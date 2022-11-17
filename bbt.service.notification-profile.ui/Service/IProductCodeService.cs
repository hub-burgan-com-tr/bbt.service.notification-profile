using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface IProductCodeService
    {
        [Get("/ProductCodes")]
      
        Task<GetProductCodeResponse> GetProductCode();
    }
}
