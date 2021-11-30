

public class GetSourcesResponse
{
    public List<Source> Sources { get; set; }
    public class Source
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string PushServiceReference { get; set; }
        public string SmsServiceReference { get; set; }
        public string EmailServiceReference { get; set; }
    }
}
