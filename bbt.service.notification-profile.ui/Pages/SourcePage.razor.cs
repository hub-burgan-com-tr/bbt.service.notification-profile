using bbt.service.notification.ui.Component;
using bbt.service.notification.ui.Configuration;
using bbt.service.notification.ui.Model;
using bbt.service.notification.ui.Service;
using Microsoft.AspNetCore.Components;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using Notification.Profile.Model.Database;
using Radzen.Blazor;


namespace bbt.service.notification.ui.Pages
{
    public partial class SourcePage : BaseComponent
    {
        [Inject]
        public ISourceService sourceService { get; set; }
        [Inject]
        public IProductCodeService productCodeService { get; set; }
        [Inject]
        public IDengageService dengageService { get; set; }
        public List<TextValueItem> displayTipList { get; set; }

        public List<Notification.Profile.Model.Source> parentList { get; set; }

        [Parameter]
        public int SourceId { get; set; }

        [Inject]
        protected IBaseConfiguration baseConfiguration { get; set; }

        [Inject]
        protected IConfiguration configuration { get; set; }
        [Parameter]
        public Notification.Profile.Model.Source sourceDetayModel { get; set; }
        [Inject]
        public Radzen.DialogService dialogService { get; set; }
        public int DisplayType { get; set; }



        //List<DengageGetContents> listContentSms { get; set; }
        List<ContentInfo> listContentSms { get; set; }
        List<ContentInfo> listContentEmail { get; set; }
        List<ContentInfo> listContentPush { get; set; }
        List<ProductCode> productCodeList { get; set; }

        public bool appsetting { get; set; }

        public SearchSourceModel searchModel { get; set; } = new SearchSourceModel();
        [Parameter]
        public EventCallback<int> ListUpdate { get; set; }
        public PostSourceRequest sourceModel { get; set; } = new PostSourceRequest();
        protected override void CustomOnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                ExecuteMethod(() =>
                {
                    LoadingModal.Open();
                    if (sourceDetayModel != null && sourceDetayModel.Id != 0 && sourceDetayModel.Id != null)
                    {
                        sourceModel.KafkaCertificate = sourceDetayModel.KafkaCertificate;
                        sourceModel.SmsServiceReference = sourceDetayModel.SmsServiceReference;
                        sourceModel.PushServiceReference = sourceDetayModel.PushServiceReference;
                        sourceModel.EmailServiceReference = sourceDetayModel.EmailServiceReference;
                        sourceModel.KafkaUrl = sourceDetayModel.KafkaUrl;
                        sourceModel.RetentationTime = sourceDetayModel.RetentationTime;
                        sourceModel.Title_TR = sourceDetayModel.Title.TR;
                        sourceModel.Title_EN = sourceDetayModel.Title.EN;
                        sourceModel.DisplayType = sourceDetayModel.DisplayType;
                        sourceModel.Secret = sourceDetayModel.Secret;
                        sourceModel.ParentId = sourceDetayModel.ParentId;
                        sourceModel.Id = sourceDetayModel.Id;
                        sourceModel.Topic = sourceDetayModel.Topic;
                        sourceModel.ClientIdJsonPath = sourceDetayModel.ClientIdJsonPath;
                        sourceModel.ProductCodeId = sourceDetayModel.ProductCodeId;
                    }
                    GetProductCodeResponse getProductCodeResponse = new GetProductCodeResponse();
                    getProductCodeResponse = productCodeService.GetProductCode().Result;
                    if (getProductCodeResponse != null && getProductCodeResponse.Result == ResultEnum.Error)
                    {
                        Notification.ShowErrorMessage("Hata", getProductCodeResponse.MessageList[0].ToString());
                    }
                    else
                    {
                        productCodeList = productCodeService.GetProductCode().Result.ProductCodes;
                    }
                    GetSourcesResponse getSourcesResponse = new GetSourcesResponse();
                    getSourcesResponse = sourceService.GetSourceWithSearchModel(searchModel).Result;
                    if (getSourcesResponse != null && getSourcesResponse.Result == ResultEnum.Error)
                    {
                        Notification.ShowErrorMessage("Hata", getSourcesResponse.MessageList[0].ToString());
                    }
                    parentList = sourceService.GetSourceWithSearchModel(searchModel).Result.Sources;
                    displayTipList = EnumHelper.BuildSelectListItems(typeof(SourceDisplayType));
                    appsetting = Convert.ToBoolean(configuration.GetSection("ProdSave").Value);
                    //  appsetting = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    GetTemplateResponseModel respEmail = dengageService.GetMessagingGatewayEmailContent().Result;
                    if (respEmail != null && respEmail.Result == ResultEnum.Success)
                    {
                        listContentEmail = respEmail.ContentList;
                    }
                    else
                    {
                        Notification.ShowErrorMessage("Hata", respEmail.MessageList[0]);
                    }
                    GetTemplateResponseModel respPush = dengageService.GetMessagingGatewayPushContent().Result;
                    if (respPush != null && respPush.Result == ResultEnum.Success)
                    {
                        listContentPush = respPush.ContentList;
                    }
                    else
                    {
                        Notification.ShowErrorMessage("Hata", respPush.MessageList[0]);
                    }
                    GetTemplateResponseModel respSms = dengageService.GetMessagingGatewaySmsContent().Result;
                    if (respSms != null && respSms.Result == ResultEnum.Success)
                    {
                        listContentSms = respSms.ContentList;
                    }
                    else
                    {
                        Notification.ShowErrorMessage("Hata", respSms.MessageList[0]);
                    }
                   
                });
                LoadingModal.Close();
            }
            base.CustomOnAfterRenderAsync(firstRender);
        }


        public void ModalClose()
        {
            dialogService.Close();

        }
        public void DisplayTypeChange()
        {

        }

        public void SourceSave()
        {
            SourceResponseModel sourceResp = new SourceResponseModel();
            if (sourceModel != null && sourceModel.Id > 0)
            {
                PatchSourceRequest patchRequest = new PatchSourceRequest();
                patchRequest.KafkaCertificate = sourceModel.KafkaCertificate;
                patchRequest.SmsServiceReference = sourceModel.SmsServiceReference;
                patchRequest.PushServiceReference = sourceModel.PushServiceReference;
                patchRequest.EmailServiceReference = sourceModel.EmailServiceReference;
                patchRequest.KafkaUrl = sourceModel.KafkaUrl;
                patchRequest.RetentationTime = sourceModel.RetentationTime;
                patchRequest.Title_TR = sourceModel.Title_TR;
                patchRequest.Title_EN = sourceModel.Title_EN;
                patchRequest.Secret = sourceModel.Secret;
                patchRequest.Topic = sourceModel.Topic;
                patchRequest.DisplayType = sourceModel.DisplayType;
                patchRequest.CheckDeploy = sourceModel.CheckDeploy;
                patchRequest.ClientIdJsonPath = sourceModel.ClientIdJsonPath;
                patchRequest.ProductCodeId = sourceModel.ProductCodeId;

                sourceResp = sourceService.Patch(sourceModel.Id, patchRequest).Result;
                if (sourceResp.Result == ResultEnum.Error)
                {

                    Notification.ShowErrorMessage("Hata", "Kaydedilirken Hata Oluştu");
                }
                else
                {
                    Notification.ShowSuccessMessage("Başarılı", "Bilgiler Başarıyla Kaydedildi");
                    dialogService.Close();

                    // string returnPage = baseConfiguration.Get<GeneralSettings>().PathBase + "Pages/SourceListPage";
                    NavigationManager.NavigateTo("Pages/SourceListPage");
                    ListUpdate.InvokeAsync();

                }
            }
            else
            {


                sourceResp = sourceService.Post(sourceModel).Result;
                if (sourceResp.Result == ResultEnum.Error)
                {
                    Notification.ShowErrorMessage("Hata", "Kaydedilirken Hata Oluştu");
                }
                else
                {
                    Notification.ShowSuccessMessage("Başarılı", "Bilgiler Başarıyla Kaydedildi");
                    dialogService.Close();
                    NavigationManager.NavigateTo("Pages/SourceListPage");
                    ListUpdate.InvokeAsync();


                }

            }
        }

    }
}
