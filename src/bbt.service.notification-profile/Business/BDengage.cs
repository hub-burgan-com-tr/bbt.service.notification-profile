using Notification.Profile.Business.Dengage.Content;
using Notification.Profile.Business.Dengage.Login;
using Notification.Profile.Model;

namespace Notification.Profile.Business
{
    public class BDengage:IDengage
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BDengage> _logger;
        public BDengage(IConfiguration configuration, ILogger<BDengage> logger) 
        {
            _configuration = configuration;
            _logger = logger; 
        }
        public GetDengageContentResponse GetDengageEmailContent()
        {
            GetDengageContentResponse bDengageEmailContentResponseModel = new GetDengageContentResponse();
            string token = Login("X").Result;
            string apiUrl= _configuration.GetValue<string>("DengageApi:Url");
            var bDengageEmailContent = new BDengageEmailContent(_logger, token, apiUrl);
             bDengageEmailContentResponseModel =  bDengageEmailContent.GetEmailContents().Result;
            if (bDengageEmailContentResponseModel != null && bDengageEmailContentResponseModel.data != null)
            {
                bDengageEmailContentResponseModel.data.result = bDengageEmailContentResponseModel.data.result.Where(x => x.isTransactionalContent == true).ToList();
            }
            return bDengageEmailContentResponseModel;
        }
        public GetDengageContentResponse GetDengageSmsContent()
        {
            GetDengageContentResponse bDengageSmsContentResponseModel = new GetDengageContentResponse();
            string token = Login("X").Result;
            string apiUrl = _configuration.GetValue<string>("DengageApi:Url");
            var bDengageSmsContent = new BDengageSmsContent(_logger, token, apiUrl);
            bDengageSmsContentResponseModel = bDengageSmsContent.GetSmsContents().Result;
            if (bDengageSmsContentResponseModel != null && bDengageSmsContentResponseModel.data != null)
            {
                bDengageSmsContentResponseModel.data.result = bDengageSmsContentResponseModel.data.result.Where(x => x.isTransactionalContent == true).ToList();
            }
            return bDengageSmsContentResponseModel;
        }
        public GetDengageContentResponse GetDengagePushContent()
        {
            GetDengageContentResponse bDengagePushContentResponseModel = new GetDengageContentResponse();
            string token = Login("X").Result;
            string apiUrl = _configuration.GetValue<string>("DengageApi:Url");
            var bDengagePushContent = new BDengagePushContent(_logger, token, apiUrl);
            bDengagePushContentResponseModel = bDengagePushContent.GetPushContents().Result;
            if (bDengagePushContentResponseModel != null && bDengagePushContentResponseModel.data != null)
            {
                bDengagePushContentResponseModel.data.result = bDengagePushContentResponseModel.data.result.Where(x => x.isTransactionalContent == true).ToList();
            }
            return bDengagePushContentResponseModel;
        }
        private async Task<string> Login(string BUSINESS_LINE)
        {
           
                string strDigital = "NoneDigital";

                if (BUSINESS_LINE == "X")
                {
                    strDigital = "Digital";
                }

                var userKey = _configuration.GetValue<string>("DengageApi:ApiKey:" + strDigital + ":UserKey");
                var pass = _configuration.GetValue<string>("DengageApi:ApiKey:" + strDigital + ":Password");
            string apiUrl = _configuration.GetValue<string>("DengageApi:Url");
            var bDengageLogin = new BDengageLogin(_logger, apiUrl);
            var dengageLoginResponseModel = await bDengageLogin.Login(new DengageLoginRequestModel()
            {
                userkey = userKey,
                password = pass
            });

            return dengageLoginResponseModel.access_token;
        }

    }
}
