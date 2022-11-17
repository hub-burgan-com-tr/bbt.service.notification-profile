using bbt.framework.dengage;
using Notification.Profile.Model;

namespace Notification.Profile.Business.Dengage.Content
{
   
    public class BDengagePushContent : BaseRefit<IDengagePushContent>
    {
        public BDengagePushContent(ILogger logger,string token, string baseURL) : base(baseURL, token, logger)
        {

        }

        public override string controllerName => "rest/contents";

        public async Task<GetDengageContentResponse> GetPushContents()
        {
            return await ExecutePolly(() =>
            {
                return api.GetPushContents().Result;
            }
            );
        }
    }
}
