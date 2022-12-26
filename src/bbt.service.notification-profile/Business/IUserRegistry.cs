using Notification.Profile.Model;

namespace Notification.Profile.Business
{
    public interface IUserRegistry
    {
        UserRegistryResponseModel PostUserRegistry(UserRegistryRequestModel request);

        UserRegistryResponseModel PatchUserRegistry(int id, string registryNo);

        UserRegistryResponseModel GetUserRegistryWithRegistryNo(string registryNo);

        UserRegistryResponseModel GetUserRegistryWithId(int Id);

        GetUserRegistryModel GetUserRegistry();

        UserRegistryResponseModel DeleteUserRegistry(int id);
    }
}
