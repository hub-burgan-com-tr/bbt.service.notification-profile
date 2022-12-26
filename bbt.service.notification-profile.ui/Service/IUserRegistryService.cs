using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface IUserRegistryService
    {

        [Get("/GetUserRegistryNo/{registryNo}")]
        Task<UserRegistryResponseModel> GetUserRegistryWithRegistryNo(string registryNo);

       
    }
}

