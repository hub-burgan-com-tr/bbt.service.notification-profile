using Notification.Profile.Model;
using Refit;

namespace bbt.service.notification.ui.Service
{
    public interface IDengageService
    {
        [Get("/dengage/email")]
        Task<GetDengageContentResponse> GetDengageEmailContent();

        [Get("/dengage/sms")]
        Task<GetDengageContentResponse> GetDengageSmsContent();

        [Get("/dengage/push")]
        Task<GetDengageContentResponse> GetDengagePushContent();

        [Get("/dengage/MessagingGatewayPush")]
        Task<GetTemplateResponseModel> GetMessagingGatewayPushContent();
        [Get("/dengage/MessagingGatewaySms")]
        Task<GetTemplateResponseModel> GetMessagingGatewaySmsContent();
        [Get("/dengage/MessagingGatewayEmail")]
        Task<GetTemplateResponseModel> GetMessagingGatewayEmailContent();
    }
}
