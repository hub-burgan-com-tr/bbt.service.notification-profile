namespace Notification.Profile.Helper
{
    public static class ConvertHelper
    {
        public static int ToInt(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch 
            {
               return 0;
            }
        }
    }
}