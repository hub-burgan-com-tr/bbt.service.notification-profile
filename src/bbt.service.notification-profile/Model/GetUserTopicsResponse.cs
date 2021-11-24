

public class GetUserTopicsResponse
{
    public List<TopicInfo> Topics { get; set; }

    public class TopicInfo
    {
        public List<KeyValuePair<string,string>> Variants { get; set; }
        public string Source { get; set; }
        public string Filter { get; set; }
        public bool IsPushEnabled { get; set; }
        public string DeviceKey { get; set; }
        public bool IsSmsEnabled { get; set; }
        public Phone Phone { get; set; }
        public bool IsMailEnabled { get; set; }
        public string Email { get; set; }
    }
}
