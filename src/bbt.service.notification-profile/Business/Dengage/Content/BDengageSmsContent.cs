using bbt.framework.dengage;
using Notification.Profile.Model;

namespace Notification.Profile.Business.Dengage.Content
{
   
    public class BDengageEmailContent : BaseRefit<IDengageEmailContent>
    {
        public BDengageEmailContent(ILogger logger,string token, string baseURL) : base(baseURL, token, logger)
        {

        }

        public override string controllerName => "rest/contents";

        public async Task<GetDengageContentResponse> GetEmailContents()
        {
            return await ExecutePolly(() =>
            {
                return api.GetEmailContents().Result;
            }
            );
        }
    }
}
