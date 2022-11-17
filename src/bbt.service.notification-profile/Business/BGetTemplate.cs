using bbt.service.notification_profile.Helper;
using Newtonsoft.Json;
using Notification.Profile.Enum;
using Notification.Profile.Helper;
using Notification.Profile.Model;
using System.Text;

namespace Notification.Profile.Business
{
    public class BGetTemplate : IGetTemplate
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public BGetTemplate(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<GetTemplateResponseModel> GetTemplateMailBurgan()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetMailBurganTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
         
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                return repsModel;

            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplateMailBurganOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            GetTemplateResponseModel mailBurganListResp =GetTemplateMailBurgan().Result;
            List<ContentInfo> mailburganlist = new List<ContentInfo>();
            List<ContentInfo> mailOnlist = new List<ContentInfo>();
            List<ContentInfo> mailBurganOnlist = new List<ContentInfo>();
            if (mailBurganListResp != null && mailBurganListResp.Result == ResultEnum.Success)
            {
                mailburganlist = mailBurganListResp.ContentList;
               
            }
            GetTemplateResponseModel mailOnListResp = GetTemplateMailOn().Result;
            if (mailOnListResp != null && mailOnListResp.Result == ResultEnum.Success)
            {
                mailOnlist = mailBurganListResp.ContentList;
            }
            mailburganlist.AddRange(mailOnlist);
            repsModel.ContentList = mailburganlist.DistinctBy(x=>x.contentName).ToList();
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplateMailOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetMailOnTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                return repsModel;

            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplatePushBurgan()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetPushBurganTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                              return repsModel;
            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }

        public  async Task<GetTemplateResponseModel> GetTemplatePushBurganOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            GetTemplateResponseModel pushBurganListResp = GetTemplatePushBurgan().Result;
            List<ContentInfo> pushburganlist = new List<ContentInfo>();
            List<ContentInfo> pushOnlist = new List<ContentInfo>();
         
            if (pushBurganListResp != null && pushBurganListResp.Result == ResultEnum.Success)
            {
                pushburganlist = pushBurganListResp.ContentList;

            }
            GetTemplateResponseModel pushOnListResp = GetTemplatePushOn().Result;
            if (pushOnListResp != null && pushOnListResp.Result == ResultEnum.Success)
            {
                pushOnlist = pushOnListResp.ContentList;
            }
            pushburganlist.AddRange(pushOnlist);
            repsModel.ContentList = pushburganlist.DistinctBy(x => x.contentName).ToList(); ;
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplatePushOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetPushOnTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                return repsModel;
            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplateSmsBurgan()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetSmsBurganTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                return repsModel;
            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplateSmsBurganOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            GetTemplateResponseModel smsBurganListResp = GetTemplateSmsBurgan().Result;
            List<ContentInfo> smsburganlist = new List<ContentInfo>();
            List<ContentInfo> smsOnlist = new List<ContentInfo>();

            if (smsBurganListResp != null && smsBurganListResp.Result == ResultEnum.Success)
            {
                smsburganlist = smsBurganListResp.ContentList;

            }
            GetTemplateResponseModel smsOnListResp = GetTemplateSmsOn().Result;
            if (smsOnListResp != null && smsOnListResp.Result == ResultEnum.Success)
            {
                smsOnlist = smsOnListResp.ContentList;
            }
            smsburganlist.AddRange(smsOnlist);
            repsModel.ContentList = smsburganlist.DistinctBy(x => x.contentName).ToList();
            return repsModel;
        }

        public async Task<GetTemplateResponseModel> GetTemplateSmsOn()
        {
            GetTemplateResponseModel repsModel = new GetTemplateResponseModel();
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            string path = configurationHelper.GetSmsOnTemplateEndpoint();
            var response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var readResp = response.Content.ReadAsByteArrayAsync().Result;
                var list = JsonConvert.DeserializeObject<List<ContentInfo>>(
                        Encoding.UTF8.GetString(readResp));
                repsModel.ContentList = list;
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode200);
                repsModel.Result = ResultEnum.Success;
                return repsModel;
            }
            else
            {
                repsModel.StatusCode = EnumHelper.GetDescription<StatusCodeEnum>(StatusCodeEnum.StatusCode472);
                repsModel.Result = ResultEnum.Error;
                repsModel.MessageList.Add(response.ReasonPhrase.ToString());
                return repsModel;
            }
            return repsModel;
        }
    }
}
