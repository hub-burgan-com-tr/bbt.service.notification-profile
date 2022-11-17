using Notification.Profile.Model;
using RestEase;

namespace Notification.Profile.Business.Dengage.Content
{
    public interface IDengagePushContent
    {
        [Header("Authorization", "Bearer")]
        [Get("/contents/push")]
        Task<GetDengageContentResponse> GetPushContents();
    }
}
