

using Notification.Profile.Model;
using System.ComponentModel;

namespace Notification.Profile.Helper
{
    public static class EnumHelper
    {

        public static string GetDescription<TEnum>(this TEnum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());

            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if(attributes.Length > 0)
                {
                    return attributes[0].Description;   
                }

            }
            return enumValue.ToString();
        }
        public static List<TextValueItem> BuildSelectListItems(Type t)
        {
            return System.Enum.GetValues(t)
                       .Cast<System.Enum>()
                       .Select(e => new TextValueItem { Value = e.GetHashCode(), Text = e.GetDescription() })
                       .ToList();
        }
    }
}

