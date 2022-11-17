using bbt.framework.dengage;
using Notification.Profile.Model;

namespace Notification.Profile.Business.Dengage.Content
{
   
    public class BDengageSmsContent : BaseRefit<IDengageSmsContent>
    {
        public BDengageSmsContent(ILogger logger,string token, string baseURL) : base(baseURL, token, logger)
        {

        }

        public override string controllerName => "rest/contents";

        public async Task<GetDengageContentResponse> GetSmsContents()
        {
            return await ExecutePolly(() =>
            {
                return api.GetSmsContents().Result;
            }
            );
        }
    }
}
