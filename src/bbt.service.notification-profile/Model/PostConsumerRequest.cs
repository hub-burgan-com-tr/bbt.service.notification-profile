

public class PostConsumerRequest
{
    public string Source { get; set; }
    public string Filter { get; set; }
    public bool IsPushEnabled { get; set; }
    public string DeviceKey { get; set; }
    public bool IsSmsEnabled { get; set; }
    public Phone Phone { get; set; }
    public bool IsEmailEnabled { get; set; }
    public string Email { get; set; }
    public List<Variant> Variants { get; set; }
    public class Variant
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
