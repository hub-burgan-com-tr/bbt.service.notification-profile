using Notification.Profile.Model;

namespace Notification.Profile.Business
{
    public interface IDengage
    {
        GetDengageContentResponse GetDengageEmailContent();
        GetDengageContentResponse GetDengageSmsContent();
        GetDengageContentResponse GetDengagePushContent();

    }
}
