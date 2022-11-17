using Notification.Profile.Model;

namespace Notification.Profile.Business

{
    public interface IinstandReminder
    {
        Task<GetInstantCustomerPermissionResponse> GetCustomerPermission(string customerId,string lang);
        Task<PostInstantCustomerPermissionResponse> PostCustomerPermission(string customerId, PostInstantCustomerPermissionRequest request);


    }
}