using Notification.Profile.Model;

namespace Notification.Profile.Business
{
    public interface IGetTemplate
    {
        Task<GetTemplateResponseModel>  GetTemplateSmsBurgan();
        Task<GetTemplateResponseModel> GetTemplateSmsOn();
        Task<GetTemplateResponseModel> GetTemplateMailBurgan();
        Task<GetTemplateResponseModel> GetTemplateMailOn();

        Task<GetTemplateResponseModel> GetTemplatePushBurgan();
       Task<GetTemplateResponseModel> GetTemplatePushOn();

        Task<GetTemplateResponseModel> GetTemplateSmsBurganOn();
        Task<GetTemplateResponseModel> GetTemplateMailBurganOn();
        Task<GetTemplateResponseModel> GetTemplatePushBurganOn();
    }
}
