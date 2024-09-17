using System.ComponentModel;

namespace Notification.Profile.Enum
{
    [Flags]
    public enum MessageDataFieldType
    {
        [Description("Json")]
        Json = 0,
        [Description("String")]
        String = 1
    }
}