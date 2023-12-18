using System.Text.Json.Serialization;

namespace Notification.Profile.Model
{
    public class ContentInfo
    {
        private string contentName_;

        public string contentName
        {
            get { return contentName_ ?? name; }
            set { contentName_ = value; }
        }

        private string publicId_;

        public string publicId
        {
            get { return publicId_ ?? id; }
            set { publicId_ = value; }
        }

        public string location { get; set; }

        [JsonIgnore]
        public string name { get; set; }

        [JsonIgnore]
        public string id { get; set; }

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
