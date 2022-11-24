namespace Notification.Profile.Model
{
    public class ContentInfo
    {
        public string contentName { get; set; }
        public string publicId { get; set; }
        public string location { get; set; }

        public string contentfullpath
        {
            get
            {
                if (location == "/")
                {
                    return location + contentName;
                }
                else
                {
                    return location + "/" + contentName;
                }

            }
        }

    }
}
