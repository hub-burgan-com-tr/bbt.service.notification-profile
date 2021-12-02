

public class Source
{
    public string Id { get; set; }
    public string Title_TR { get; set; }
    public string Title_EN { get; set; }
    public string ParentId { get; set; }
    public string Topic { get; set; }
    public string ApiKey { get; set; }
    public string Secret { get; set; }
    public List<SourceParameter> SourceParameters { get; set; }
    public string PushServiceReference { get; set; }
    public string SmsServiceReference { get; set; }
    public string EmailServiceReference { get; set; }

}
