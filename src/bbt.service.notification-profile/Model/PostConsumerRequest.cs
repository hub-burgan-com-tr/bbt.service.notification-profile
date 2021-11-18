

public class PostConsumerRequest
{
    public Guid SourceId { get; set; }
    public long User { get; set; }
    public string Path { get; set; }
    public string Filter { get; set; }
    public bool IsPushEnabled { get; set; }
    public string DeviceKey { get; set; }
    public bool IsSmsEnabled { get; set; }
    public Phone Phone { get; set; }
    public bool IsMailEnabled { get; set; }
    public string Email { get; set; }
}
