

public class GetConsumerByTopicResponse
{

    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public string SourceTitle { get; set; }
    public long User { get; set; }
    public string Topic { get; set; }
    public string Filter { get; set; }
    public bool IsPushEnabled { get; set; }
    public string DeviceKey { get; set; }
    public bool IsSmsEnabled { get; set; }
    public Phone Phone { get; set; }
    public bool IsMailEnabled { get; set; }
    public string Email { get; set; }

}
