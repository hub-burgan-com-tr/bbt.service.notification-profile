using bbt.framework.dengage;

namespace Notification.Profile.Business.Dengage.Login
{
    public class BDengageLogin : BaseRefit<IDengageLogin>
    {
        public BDengageLogin(ILogger logger, string baseURL) : base(baseURL, string.Empty, logger)
        {

        }

        public override string controllerName => "login";

        public async Task<DengageLoginResponseModel> Login(DengageLoginRequestModel request)
        {
            return await ExecutePolly(() =>
            {
                return api.Login(request).Result;
            }
            );
        }
    }
}
