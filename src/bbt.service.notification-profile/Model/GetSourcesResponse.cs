

public class GetSourcesResponse
{
    public List<Source> Sources { get; set; }
    public class Source
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public string ConsumerGroup { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public string PushServiceReference { get; set; }
        public bool SmsServiceReference { get; set; }
        public string MailServiceReference { get; set; }
    }
}
