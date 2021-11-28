

public class Consumer
{
    
    public Guid Id { get; set; }
    public Source Source { get; set; }
    public string SourceId { get; set; }
    public long Client { get; set; }
    public long User { get; set; }
    public List<ConsumerVariant> Variants { get; set; }
    public string Filter { get; set; }
    public bool IsPushEnabled { get; set; }
    public string DeviceKey { get; set; }
    public bool IsSmsEnabled { get; set; }
    public Phone Phone { get; set; }
    public bool IsMailEnabled { get; set; }
    public string Email { get; set; }

}
