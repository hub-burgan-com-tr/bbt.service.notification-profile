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
                if (attributes.Length > 0)
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

        public static int[] ToIntArray(this System.Enum o)
        {
            return o.ToString()
                .Split(new string[] { ", " }, StringSplitOptions.None)
                .Select(i => (int)System.Enum.Parse(o.GetType(), i))
                .ToArray();
        }

        public static object[] ToEnumArray(this System.Enum o)
        {
            return o.ToString()
                .Split(new string[] { ", " }, StringSplitOptions.None)
                .Select(i => System.Enum.Parse(o.GetType(), i))
                .ToArray();
        }

        public static int EnumListToInt(IEnumerable<System.Enum> list)
        {
            var retVal = 0;
            if (list != null)
            {
                foreach (var item in list)
                {
                    retVal += (int)System.Enum.ToObject(item.GetType(), item);
                }

            }
            return retVal;
        }

        public static int IntListToInt(IEnumerable<int> list)
        {
            var retVal = 0;
            if (list !=null)
            {
                foreach (var item in list)
                {
                    retVal += item;
                }            
            }
            return retVal;
        }
    }
}