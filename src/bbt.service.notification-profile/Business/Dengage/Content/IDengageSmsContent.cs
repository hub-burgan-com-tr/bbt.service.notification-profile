using Notification.Profile.Model;
using RestEase;

namespace Notification.Profile.Business.Dengage.Content
{
    public interface IDengageSmsContent
    {
        [Header("Authorization", "Bearer")]
        [Get("/contents/sms")]
        Task<GetDengageContentResponse> GetSmsContents();
    }
}
