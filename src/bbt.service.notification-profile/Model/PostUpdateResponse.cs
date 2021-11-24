

public class PostUpdateResponse
{
    public List<TopicInfo> UpdatedTopics { get; set; }

    public class TopicInfo
    {
        public string Source { get; set; }
        public long Client { get; set; }
    }

}
