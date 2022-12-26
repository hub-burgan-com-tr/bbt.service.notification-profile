namespace bbt.service.notification.ui.Base.Token
{
    public interface ITokenService
    {
        Task<string> GetToken();
        OktaSettings GetOktaSettings();
    }
}
