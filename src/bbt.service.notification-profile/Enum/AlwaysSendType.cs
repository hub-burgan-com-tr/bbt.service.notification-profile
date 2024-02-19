namespace Notification.Profile.Enum
{
    [Flags]
    public enum AlwaysSendType
    {
        None = 0,
        Sms = 1,
        EMail = 2,
        Push = 4,
    }
}