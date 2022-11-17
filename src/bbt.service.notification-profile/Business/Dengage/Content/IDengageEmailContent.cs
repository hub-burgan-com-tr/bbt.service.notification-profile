using Notification.Profile.Model;
using RestEase;

namespace Notification.Profile.Business.Dengage.Content
{
    public interface IDengageEmailContent
    {
        [Header("Authorization", "Bearer")]
        [Get("/contents/email")]
        Task<GetDengageContentResponse> GetEmailContents();
    }
}
